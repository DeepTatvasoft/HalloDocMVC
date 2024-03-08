﻿using Data.DataModels;
using HalloDoc.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Implementation
{
    public class AdminFunction : IAdminFunction
    {
        private readonly ApplicationDbContext _context;

        public AdminFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public (bool, string, int) loginadmin([Bind("Email,Passwordhash")] Aspnetuser aspNetUser)
        {
            var obj = _context.Aspnetusers.FirstOrDefault(u => u.Email == aspNetUser.Email && u.Passwordhash == aspNetUser.Passwordhash);
            if (obj != null)
            {
                var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == obj.Id.ToString());
                int id = obj.Id;
                if (admin == null)
                {
                    return (false, null, id);
                }
                else
                {
                    return (true, obj.Username, id);
                }
            }
            else
            {
                return (false, null, 0);
            }
        }
        public NewStateData AdminDashboarddata(int status1, int status2, int status3, int currentPage)
        {
            NewStateData data = new NewStateData();
            List<Request> req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Where(u => u.Status == status1 || u.Status == status2 || u.Status == status3).ToList();
            List<Request> newreq = req.Skip((currentPage - 1) * 7).Take(7).ToList();
            data.req = newreq;
            data.newcount = getNewRequestCount();
            data.activecount = getActiveRequestCount();
            data.pendingcount = getPendingRequestCount();
            data.Toclosecount = getToCloseRequestCount();
            data.concludecount = getConcludeRequestCount();
            data.Unpaidcount = getUnpaidRequestCount();
            var regions = _context.Regions.ToList();
            data.regions = regions;
            data.totalpages = req.Count();
            var casetag = _context.Casetags.ToList();
            List<Requeststatuslog> requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == status1 || u.Status == status2 || u.Status == status3).ToList();
            data.requeststatuslogs = requeststatuslogs;
            data.casetags = casetag;
            return data;
        }
        public NewStateData AdminDashboard()
        {
            NewStateData modal = new NewStateData();
            modal.concludecount = getConcludeRequestCount();
            modal.newcount = getNewRequestCount();
            modal.pendingcount = getPendingRequestCount();
            modal.activecount = getActiveRequestCount();
            modal.Toclosecount = getToCloseRequestCount();
            modal.Unpaidcount = getUnpaidRequestCount();
            return modal;
        }
        public NewStateData toogletable(string reqtypeid, string status)
        {
            NewStateData newStateData = new NewStateData();
            List<Request> req;
            List<Requeststatuslog> requeststatuslogs;
            if (status == "4")
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 4 || u.Status == 5).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == "3")
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Where(u => u.Requesttypeid.ToString() == reqtypeid && (u.Status == 3 || u.Status == 7 || u.Status == 8)).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status.ToString() == status).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status.ToString() == status).ToList();
            }
            newStateData.req = req;
            newStateData.totalpages = req.Count();
            newStateData.requeststatuslogs = requeststatuslogs;
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;

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
            var regionselect = _context.Regions.ToList();
            newStateData1.regions = regionselect;
            return newStateData1;
        }

        public NewStateData regiontable(int regionid, string status)
        {
            NewStateData newStateData = new NewStateData();
            var reqclient = _context.Requestclients.Include(m => m.Request).Where(u => u.Regionid == regionid).ToList();
            List<Requeststatuslog> requeststatuslogs;
            newStateData.requestclients = reqclient;
            List<Request> req = new List<Request>();
            foreach (var obj in reqclient)
            {
                req.Add(obj.Request);
            }
            if (status == "4")
            {
                req = req.Where(u => u.Status == 4 || u.Status == 5).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == "3")
            {
                req = req.Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
            }
            else
            {
                req = req.Where(u => u.Status.ToString() == status).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status.ToString() == status).ToList();
            }
            newStateData.req = req;
            newStateData.totalpages = req.Count();
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;
            newStateData.requeststatuslogs = requeststatuslogs;
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
        public void cancelcase(int reqid, int casetagid, string cancelnotes, string adminname, int id)
        {
            if (cancelnotes != null && casetagid != null)
            {
                var request = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
                request.Modifieddate = DateTime.Now;
                request.Declinedby = adminname;
                string casetag = _context.Casetags.FirstOrDefault(u => u.Casetagid == casetagid).Name;
                request.Casetag = casetag;
                request.Status = 3;
                _context.Requests.Update(request);
                _context.SaveChanges();
                var admin = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == id.ToString());

                Requeststatuslog requeststatuslog = new Requeststatuslog
                {
                    Requestid = reqid,
                    Status = 3,
                    Adminid = admin.Adminid,
                    Notes = cancelnotes,
                    Createddate = DateTime.Now,

                };
                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }
        }
        public List<Physician> filterregion(string regionid)
        {
            List<Physician> physicians = _context.Physicians.Where(u => u.Regionid.ToString() == regionid).ToList();
            return physicians;
        }
        public void assigncase(int reqid, int regid, int phyid, string Assignnotes, string adminname, int id)
        {
            if (phyid != 0)
            {
                var req = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
                req.Modifieddate = DateTime.Now;
                req.Status = 2;
                req.Physicianid = phyid;
                _context.Requests.Update(req);
                Requeststatuslog requeststatuslog = new Requeststatuslog
                {
                    Requestid = reqid,
                    Status = 2,
                    Adminid = id,
                    Transtophysicianid = phyid,
                    Notes = Assignnotes,
                    Createddate = DateTime.Now,
                };
                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }
        }
        public void blockcase(int reqid, string Blocknotes)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
            req.Status = 200;
            _context.Update(req);
            _context.SaveChanges();
            Blockrequest blockrequest = new Blockrequest
            {
                Phonenumber = req.Phonenumber,
                Email = req.Email,
                Reason = Blocknotes,
                Requestid = reqid.ToString(),
                Createddate = DateTime.Now,
            };
            _context.Blockrequests.Add(blockrequest);
            _context.SaveChanges();
        }
        public ViewNotesModel ViewNotes(int reqid)
        {
            ViewNotesModel viewNotesModel = new ViewNotesModel();
            var reqstatuslog = _context.Requeststatuslogs.FirstOrDefault(u => u.Requestid == reqid);
            var requestnotes = _context.Requestnotes.FirstOrDefault(u => u.Requestid == reqid);
            if (reqstatuslog != null)
            {
                string adminname = _context.Admins.FirstOrDefault(u => u.Adminid == reqstatuslog.Adminid).Firstname;
                viewNotesModel.adminname = adminname;
                string phyname;
                if (reqstatuslog.Transtophysicianid != null)
                {
                    phyname = _context.Physicians.FirstOrDefault(u => u.Physicianid == reqstatuslog.Transtophysicianid).Firstname;
                    viewNotesModel.phyname = phyname;

                }
                viewNotesModel.requeststatuslogs = reqstatuslog;
            }
            if (requestnotes != null)
            {
                viewNotesModel.adminnotes = requestnotes.Adminnotes;
            }
            viewNotesModel.reqid = reqid;
            return viewNotesModel;
        }
        public void AdminNotesSaveChanges(int reqid, string adminnotes, string adminname)
        {
            var reqnotes = _context.Requestnotes.FirstOrDefault(u => u.Requestid == reqid);
            if (reqnotes == null)
            {
                Requestnote requestnote = new Requestnote
                {
                    Requestid = reqid,
                    Adminnotes = adminnotes,
                    Createdby = adminname,
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(requestnote);
                _context.SaveChanges();
            }
            else
            {
                reqnotes.Adminnotes = adminnotes;
                _context.Requestnotes.Update(reqnotes);
                _context.SaveChanges();
            }
        }

        public AdminviewDoc AdminuploadDoc(int reqid)
        {
            AdminviewDoc adminviewDoc = new AdminviewDoc();
            adminviewDoc.Username = _context.Requests.FirstOrDefault(u => u.Requestid == reqid).Firstname;
            adminviewDoc.ConfirmationNum = _context.Requests.FirstOrDefault(u => u.Requestid == reqid).Confirmationnumber;
            var reqfile = _context.Requestwisefiles.Where(u => u.Requestid == reqid && u.Isdeleted != new BitArray(new[] { true })).ToList();
            adminviewDoc.reqfile = reqfile;
            adminviewDoc.reqid = reqid;
            return adminviewDoc;
        }

        public int SingleDelete(int reqfileid)
        {
            var requestwisefile = _context.Requestwisefiles.FirstOrDefault(u => u.Requestwisefileid == reqfileid);
            int reqid = requestwisefile.Requestid;
            requestwisefile.Isdeleted = new BitArray(new[] { true });
            _context.Requestwisefiles.Update(requestwisefile);
            _context.SaveChanges();
            return reqid;
        }
        public List<string> SendMail(List<int> reqwiseid, int reqid)
        {

            List<string> filenames = new List<string>();
            foreach (var item in reqwiseid)
            {
                var file = _context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == item).Filename;
                filenames.Add(file);
            }
            return filenames;
        }
        public List<Healthprofessionaltype> getprofession()
        {
            List<Healthprofessionaltype> list = _context.Healthprofessionaltypes.ToList();
            return list;
        }
        public List<Healthprofessional> filterprofession(int professionid)
        {
            List<Healthprofessional> list = _context.Healthprofessionals.Where(u => u.Profession == professionid).ToList();
            return list;
        }
        public Healthprofessional filterbusiness(int vendorid)
        {
            Healthprofessional profession = _context.Healthprofessionals.FirstOrDefault(u => u.Vendorid == vendorid);
            return profession;
        }
        public void OrderSubmit(SendOrders sendorder)
        {
            Orderdetail orderdetail = new Orderdetail
            {
                Vendorid = sendorder.vendorid,
                Requestid = sendorder.reqid,
                Faxnumber = sendorder.Fax,
                Email = sendorder.Email,
                Businesscontact = sendorder.Contact,
                Prescription = sendorder.prescription,
                Noofrefill = sendorder.refil,
                Createddate = DateTime.Now,
                Createdby = sendorder.createdby
            };
            _context.Orderdetails.Add(orderdetail);
            _context.SaveChanges();
        }
        public void clearcase(int reqid)
        {
            var req = _context.Requests.FirstOrDefault(u=>u.Requestid == reqid);
            req.Status = 10;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }
    }
}
