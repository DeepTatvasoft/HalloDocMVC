using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class AdminFunction : IAdminFunction
    {
        private readonly ApplicationDbContext _context;

        public AdminFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public (bool, string) loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj != null)
            {
                var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == obj.Id.ToString());
                if (admin == null)
                {
                    return (false, null);
                }
                else
                {
                    return (true, obj.Username);
                }
            }
            else
            {
                return (false, null);
            }
        }
        public NewStateData AdminDashboarddata(int status1, int status2, int status3)
        {
            NewStateData data = new NewStateData();
            List<Request> req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Status == status1 || u.Status == status2 || u.Status == status3).ToList();
            data.req = req;
            data.newcount = getNewRequestCount();
            data.activecount = getActiveRequestCount();
            data.pendingcount = getPendingRequestCount();
            data.Toclosecount = getToCloseRequestCount();
            data.concludecount = getConcludeRequestCount();
            data.Unpaidcount = getUnpaidRequestCount();
            return data;
        }
        public NewStateData toogletable(string reqtypeid, string status)
        {
            NewStateData newStateData = new NewStateData();
            List<Request> req;
            if (status == "4")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == "3")
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && (u.Status == 3 || u.Status == 7 || u.Status == 8)).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status.ToString() == status).ToList();
            }
            newStateData.req = req;
            return newStateData;
        }
        public NewStateData1 ViewCase(int id)
        {
            NewStateData1 newStateData1 = new NewStateData1();
            Request req = _context.Requests.Include(r => r.Requestclients).FirstOrDefault(u => u.Requestid == id);
            newStateData1.req = req;
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == req.Requestid);
            int date = (int)reqclient.Intdate;
            int year = (int)reqclient.Intyear;
            string month = reqclient.Strmonth.ToString();
            var region = _context.Regions.FirstOrDefault(u => u.Regionid == reqclient.Regionid);
            newStateData1.region = region.Name;
            newStateData1.DateOnly = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), (int)date).Date;
            newStateData1.address = reqclient.Address;
            newStateData1.room = reqclient.Location;
            newStateData1.symptoms = reqclient.Notes;
            return newStateData1;
        }

        public NewStateData regiontable(int regionid, string status)
        {
            NewStateData newStateData = new NewStateData();
            var reqclient = _context.Requestclients.Include(m => m.Request).Where(u => u.Regionid == regionid).ToList();
            newStateData.requestclients = reqclient;
            List<Request> req = new List<Request>();
            foreach (var obj in reqclient)
            {
                req.Add(obj.Request);
            }
            if (status == "4")
            {
                req = req.Where(u => u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == "3")
            {
                req = req.Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
            }
            else
            {
                req = req.Where(u => u.Status.ToString() == status).ToList();
            }
            newStateData.req = req;
            return newStateData;
        }
        public int getToCloseRequestCount()
        {
            return _context.Requests.Count(rc => rc.Status == 3 || rc.Status == 7 || rc.Status == 8);
        }

        public int getActiveRequestCount()
        {
            return _context.Requests.Count(rc => rc.Status == 4 || rc.Status == 5);
        }

        public int getConcludeRequestCount()
        {
            return _context.Requests.Count(rc => rc.Status == 6);
        }

        public int getNewRequestCount()
        {
            return _context.Requests.Count(r => r.Status == 1);
        }

        public int getPendingRequestCount()
        {
            return _context.Requests.Count(rc => rc.Status == 2);
        }

        public int getUnpaidRequestCount()
        {
            return _context.Requests.Count(rc => rc.Status == 9);
        }
    }
}
