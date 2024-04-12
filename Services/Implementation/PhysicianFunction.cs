using Data.DataContext;
using Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class PhysicianFunction : IPhysicianFunction
    {
        private readonly ApplicationDbContext _context;

        public PhysicianFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public NewStateData AdminDashboarddata(int status, int currentPage, int phyid, string searchkey = "")
        {
            NewStateData data = new NewStateData();
            List<Request> req = new List<Request>();
            if (status == 1)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null).ToList();
            }
            else if (status == 2)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else if (status == 4)
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => (u.Status == 4 || u.Status == 5) && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == 6 && u.Physicianid == phyid).ToList();
            }
            data.newcount = getNewRequestCount(phyid);
            data.activecount = getActiveRequestCount(phyid);
            data.pendingcount = getPendingRequestCount(phyid);
            data.concludecount = getConcludeRequestCount(phyid);
            var regions = _context.Regions.ToList();
            data.regions = regions;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            data.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            var casetag = _context.Casetags.ToList();
            data.req = req;
            data.casetags = casetag;
            data.currentpage = currentPage;
            data.searchkey = searchkey;
            data.status = Convert.ToInt32(status);
            return data;
        }

        public int getActiveRequestCount(int phyid)
        {
            return _context.Requests.Count(rc => (rc.Status == 4 || rc.Status == 5) && rc.Physicianid == phyid && rc.Accepteddate != null);
        }

        public int getConcludeRequestCount(int phyid)
        {
            return _context.Requests.Count(u => u.Status == 6 && u.Physicianid == phyid);

        }

        public int getNewRequestCount(int phyid)
        {
            return _context.Requests.Count(u => u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null);
        }

        public int getPendingRequestCount(int phyid)
        {
            return _context.Requests.Count(rc => rc.Status == 2 && rc.Physicianid == phyid && rc.Accepteddate != null);
        }
        public NewStateData AdminDashboard(int phyid)
        {
            NewStateData modal = new NewStateData();
            modal.concludecount = getConcludeRequestCount(phyid);
            modal.newcount = getNewRequestCount(phyid);
            modal.pendingcount = getPendingRequestCount(phyid);
            modal.activecount = getActiveRequestCount(phyid);
            return modal;
        }
        public NewStateData toogletable(string reqtypeid, string status, int currentPage, int phyid, string searchkey = "")
        {
            NewStateData newStateData = new NewStateData();
            List<Request> req = new List<Request>();
            if (status == "1")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 2 && u.Physicianid == phyid && u.Accepteddate == null).ToList();
            }
            else if (status == "2")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 2 && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else if (status == "4")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && (u.Status == 4 || u.Status == 5) && u.Physicianid == phyid && u.Accepteddate != null).ToList();
            }
            else
            {              
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 6 && u.Physicianid == phyid).ToList();
            }
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;
            newStateData.reqtype = reqtypeid;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            newStateData.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            newStateData.currentpage = currentPage;
            newStateData.req = req;
            newStateData.searchkey = searchkey;
            newStateData.status = Convert.ToInt32(status);
            return newStateData;
        }
        public void PhysicianAccept(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            req.Accepteddate = DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }
    }
}
