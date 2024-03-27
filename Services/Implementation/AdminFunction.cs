using Data.DataContext;
using Data.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using System.Net.Mail;
using System.Net;

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
        public NewStateData AdminDashboarddata(int status, int currentPage, string searchkey = "")
        {
            NewStateData data = new NewStateData();
            List<Request> req = new List<Request>();
            List<Requeststatuslog> requeststatuslogs = new List<Requeststatuslog>();
            if (status == 4)
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Include(z => z.Physician).Where(u => u.Status == 4 || u.Status == 5).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == 3)
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Include(z => z.Physician).Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
            }
            else
            {
                req = _context.Requests.Include(r => r.Requestclients).Include(m => m.Requeststatuslogs).Include(z => z.Physician).Where(u => u.Status == status).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == status).ToList();
            }
            data.newcount = getNewRequestCount();
            data.activecount = getActiveRequestCount();
            data.pendingcount = getPendingRequestCount();
            data.Toclosecount = getToCloseRequestCount();
            data.concludecount = getConcludeRequestCount();
            data.Unpaidcount = getUnpaidRequestCount();
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
            data.requeststatuslogs = requeststatuslogs;
            data.casetags = casetag;
            data.currentpage = currentPage;
            data.searchkey = searchkey;
            data.status = Convert.ToInt32(status);
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
            modal.regions = _context.Regions.ToList();
            return modal;
        }
        public NewStateData toogletable(string reqtypeid, string status, int currentPage, string searchkey = "")
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
            newStateData.requeststatuslogs = requeststatuslogs;
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
        public NewStateData RegionReqtype(int regionid, string reqtypeid, string status, int currentPage, string searchkey = "")
        {
            NewStateData newStateData = new NewStateData();
            newStateData.region = regionid;
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
                req = req.Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 4 || u.Status == 5).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 4 || u.Status == 5).ToList();
            }
            else if (status == "3")
            {
                req = req.Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status == 3 || u.Status == 7 || u.Status == 8).ToList();
            }
            else
            {
                req = req.Where(u => u.Requesttypeid.ToString() == reqtypeid && u.Status.ToString() == status).ToList();
                requeststatuslogs = _context.Requeststatuslogs.Include(r => r.Transtophysician).Where(u => u.Status.ToString() == status).ToList();
            }
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;
            newStateData.reqtype = reqtypeid;
            newStateData.requeststatuslogs = requeststatuslogs;
            newStateData.currentpage = currentPage;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            newStateData.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            newStateData.req = req;
            newStateData.searchkey = searchkey;
            newStateData.status = Convert.ToInt32(status);
            return newStateData;
        }
        public NewStateData1 ViewCase(int id)
        {
            NewStateData1 newStateData1 = new NewStateData1();
            Request? req = _context.Requests.Include(r => r.Requestclients).FirstOrDefault(u => u.Requestid == id);
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

        public NewStateData regiontable(int regionid, string status, int currentPage, string searchkey = "")
        {
            NewStateData newStateData = new NewStateData();
            newStateData.region = regionid;
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
            var regions = _context.Regions.ToList();
            newStateData.regions = regions;
            var casetag = _context.Casetags.ToList();
            newStateData.casetags = casetag;
            newStateData.currentpage = currentPage;
            newStateData.requeststatuslogs = requeststatuslogs;
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                req = req.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchkey.ToLower()) || rc.Lastname.ToLower().Contains(searchkey.ToLower()))).ToList();
            }
            newStateData.totalpages = (int)Math.Ceiling(req.Count() / 5.00);
            if (currentPage != 0)
            {
                req = req.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            newStateData.req = req;
            newStateData.searchkey = searchkey;
            newStateData.status = Convert.ToInt32(status);
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
            List<Physician> physicians = _context.Physicianregions.Where(u => u.Regionid.ToString() == regionid).Select(y => y.Physician).ToList();
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
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == reqid);
            adminviewDoc.firstname = reqclient.Firstname;
            adminviewDoc.lastname = reqclient.Lastname;
            DateTime tempDateTime = new DateTime(Convert.ToInt32(reqclient.Intyear), Convert.ToInt32(reqclient.Strmonth), (int)reqclient.Intdate);
            adminviewDoc.DOB = tempDateTime;
            adminviewDoc.phonenumber = reqclient.Phonenumber;
            adminviewDoc.email = reqclient.Email;
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
        public Healthprofessional? filterbusiness(int vendorid)
        {
            Healthprofessional? profession = _context.Healthprofessionals.FirstOrDefault(u => u.Vendorid == vendorid);
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
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == reqid);
            req.Status = 10;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }
        public void AcceptAgreement(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            req.Status = 4;
            req.Accepteddate = DateTime.Now;
            _context.Requests.Update(req);
            Requeststatuslog reqstatuslog = new Requeststatuslog
            {
                Requestid = id,
                Status = 4,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(reqstatuslog);
            _context.SaveChanges();
        }
        public void CancelAgreement(Agreementmodal modal)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == modal.reqid);
            req.Status = 3;
            req.Accepteddate = DateTime.Now;
            _context.Requests.Update(req);
            Requeststatuslog reqstatuslog = new Requeststatuslog
            {
                Requestid = modal.reqid,
                Status = 7,
                Createddate = DateTime.Now,
                Notes = modal.reason
            };
            _context.Requeststatuslogs.Add(reqstatuslog);
            _context.SaveChanges();
        }
        public void CloseCasebtn(int id)
        {
            var req = _context.Requests.FirstOrDefault(u => u.Requestid == id);
            req.Status = 9;
            req.Modifieddate = DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = id,
                Status = 9,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
        }
        public void Closecaseedit([FromForm] AdminviewDoc formData)
        {
            var reqclient = _context.Requestclients.FirstOrDefault(u => u.Requestid == formData.reqid);
            reqclient.Firstname = formData.firstname;
            reqclient.Lastname = formData.lastname;
            reqclient.Intdate = formData.DOB.Day;
            reqclient.Strmonth = formData.DOB.Month.ToString();
            reqclient.Intyear = formData.DOB.Year;
            reqclient.Email = formData.email;
            reqclient.Phonenumber = formData.phonenumber;
            _context.Requestclients.Update(reqclient);
            _context.SaveChanges();
        }
        public void AdminResetPassword(AdminProfile modal)
        {
            var aspid = _context.Admins.FirstOrDefault(u => u.Adminid == modal.adminid).Aspnetuserid;
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(u => u.Id.ToString() == aspid);
            aspnetuser.Passwordhash = modal.ResetPassword;
            _context.Aspnetusers.Update(aspnetuser);
            _context.SaveChanges();
        }
        public AdminProfile Profiletab(int adminid)
        {
            AdminProfile adminProfile = new AdminProfile();
            Admin admindata = _context.Admins.FirstOrDefault(u => u.Adminid == adminid);
            adminProfile.Username = _context.Aspnetusers.FirstOrDefault(u => u.Id.ToString() == admindata.Aspnetuserid).Username;
            adminProfile.firstname = admindata.Firstname;
            adminProfile.lastname = admindata.Lastname;
            adminProfile.email = admindata.Email;
            adminProfile.confirmemail = admindata.Email;
            adminProfile.phonenumber = admindata.Mobile;
            adminProfile.address1 = admindata.Address1;
            adminProfile.address2 = admindata.Address2;
            adminProfile.city = admindata.City;
            adminProfile.regionid = admindata.Regionid;
            adminProfile.zipcode = admindata.Zip;
            adminProfile.altphone = admindata.Altphone;
            adminProfile.status = (int)admindata.Status;
            adminProfile.regions = _context.Regions.ToList();
            adminProfile.adminid = adminid;
            adminProfile.roleid = admindata.Roleid;
            HashSet<Region> adminregion = _context.Adminregions.Include(u => u.Region).Where(u => u.Adminid == adminid).Select(u => u.Region).ToHashSet();
            adminProfile.adminregions = adminregion;
            adminProfile.roles = _context.Roles.Where(u => u.Accounttype == 1).ToList();
            return adminProfile;
        }
        public bool AdministratorinfoEdit(AdminProfile Modal, List<string> chk)
        {
            bool flag = true;
            var admin = _context.Admins.FirstOrDefault(u => u.Adminid == Modal.adminid);
            var checkmail = _context.Aspnetusers.FirstOrDefault(u => u.Email == Modal.email);
            if (checkmail != null)
            {
                flag = false;
            }
            if (Modal.email == admin.Email)
            {
                flag = true;
            }
            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Id == Modal.adminid);
            admin.Firstname = Modal.firstname;
            admin.Lastname = Modal.lastname;
            if (flag == true)
            {
                admin.Email = Modal.email;
            }
            admin.Mobile = Modal.phonenumber;
            admin.Modifieddate = DateTime.Now;
            aspuser.Phonenumber = Modal.phonenumber;
            aspuser.Email = Modal.email;
            aspuser.Modifieddate = DateTime.Now;
            _context.Aspnetusers.Update(aspuser);
            _context.Admins.Update(admin);
            var adminregion = _context.Adminregions.Where(u => u.Adminid == Modal.adminid).ToList();
            foreach (var obj in adminregion)
            {
                _context.Adminregions.Remove(obj);
            }
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Adminregion newAdminRegion = new Adminregion
                {
                    Adminid = Modal.adminid,
                    Regionid = s,
                };
                _context.Adminregions.Add(newAdminRegion);
                _context.SaveChanges();
            }
            _context.SaveChanges();
            return flag;
        }
        public void MailinginfoEdit(AdminProfile modal)
        {
            var admin = _context.Admins.FirstOrDefault(u => u.Adminid == modal.adminid);
            admin.Address1 = modal.address1;
            admin.Address2 = modal.address2;
            admin.City = modal.city;
            admin.Regionid = modal.regionid;
            admin.Zip = modal.zipcode;
            admin.Altphone = modal.altphone;
            admin.Modifieddate = DateTime.Now;
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }
        public byte[] DownloadExcle(NewStateData model)
        {
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Request Id");
                headerRow.CreateCell(2).SetCellValue("Patient Name");
                headerRow.CreateCell(3).SetCellValue("Patient DOB");
                headerRow.CreateCell(4).SetCellValue("RequestorName");
                headerRow.CreateCell(5).SetCellValue("RequestedDate");
                headerRow.CreateCell(6).SetCellValue("PatientPhone");
                headerRow.CreateCell(7).SetCellValue("TransferNotes");
                headerRow.CreateCell(8).SetCellValue("RequestorPhone");
                headerRow.CreateCell(9).SetCellValue("RequestorEmail");
                headerRow.CreateCell(10).SetCellValue("Address");
                headerRow.CreateCell(11).SetCellValue("Notes");
                headerRow.CreateCell(12).SetCellValue("ProviderEmail");
                headerRow.CreateCell(13).SetCellValue("PatientEmail");
                headerRow.CreateCell(14).SetCellValue("RequestType");
                headerRow.CreateCell(15).SetCellValue("Region");
                headerRow.CreateCell(16).SetCellValue("PhysicainName");
                headerRow.CreateCell(17).SetCellValue("Status");

                for (int i = 0; i < model.req.Count; i++)
                {
                    var reqclient = model.req.ElementAt(i).Requestclients.ElementAt(0);
                    var type = "";
                    if (model.req.ElementAt(i).Requesttypeid == 1)
                    {
                        type = "Patient";
                    }
                    else if (model.req.ElementAt(i).Requesttypeid == 2)
                    {
                        type = "Family";
                    }
                    else if (model.req.ElementAt(i).Requesttypeid == 4)
                    {
                        type = "Business";
                    }
                    else if (model.req.ElementAt(i).Requesttypeid == 3)
                    {
                        type = "Concierge";
                    }
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(model.req.ElementAt(i).Requestid);
                    row.CreateCell(2).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Firstname);
                    row.CreateCell(3).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Intdate + "/" + model.req.ElementAt(i).Requestclients.ElementAt(0).Strmonth + "/" + model.req.ElementAt(i).Requestclients.ElementAt(0).Intyear);
                    row.CreateCell(4).SetCellValue(model.req.ElementAt(i).Firstname);
                    row.CreateCell(5).SetCellValue(model.req.ElementAt(i).Createddate);
                    row.CreateCell(6).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Phonenumber);
                    if (model.requeststatuslogs.Count == 0)
                    {
                        row.CreateCell(7).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(model.requeststatuslogs.ElementAt(0).Notes);
                    }
                    row.CreateCell(8).SetCellValue(model.req.ElementAt(i).Phonenumber);
                    row.CreateCell(9).SetCellValue(model.req.ElementAt(i).Email);
                    row.CreateCell(10).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Address);
                    row.CreateCell(11).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Notes);
                    if (model.physicians == null)
                    {
                        row.CreateCell(12).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue(model.physicians.ElementAt(i).Email);
                    }
                    row.CreateCell(13).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Email);
                    row.CreateCell(14).SetCellValue(type);
                    row.CreateCell(15).SetCellValue(model.req.ElementAt(i).Requestclients.ElementAt(0).Region.Name);
                    if (model.req.ElementAt(i).Physician == null)
                    {
                        row.CreateCell(16).SetCellValue("");
                    }
                    else
                    {
                        row.CreateCell(16).SetCellValue(model.req.ElementAt(i).Physician.Firstname);
                    }
                    row.CreateCell(17).SetCellValue(model.status);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }
        public ProviderModal Providertab(int regionid)
        {
            ProviderModal modal = new ProviderModal();
            var physician = _context.Physicians.Include(r => r.Role).Where(u => u.Isdeleted == new BitArray(new[] { false })).ToList();
            modal.regions = _context.Regions.ToList();
            if (regionid != 0)
            {
                physician = physician.Where(u => u.Regionid == regionid).ToList();
            }
            modal.physicians = physician;
            List<int> phynotificationid = _context.Physiciannotifications.Where(u => u.Isnotificationstopped == new BitArray(new[] { true })).Select(u => u.Physicianid).ToList();
            modal.physiciannotificationid = phynotificationid;
            return modal;
        }
        public Agreementmodal ReviewAgreement(int id)
        {
            Agreementmodal modal = new Agreementmodal();
            modal.reqid = id;
            modal.firstname = _context.Requests.FirstOrDefault(u => u.Requestid == id).Firstname;
            modal.lastname = _context.Requests.FirstOrDefault(u => u.Requestid == id).Lastname;
            return modal;
        }

        public EditPhysicianModal EditPhysician(int id)
        {
            EditPhysicianModal modal = new EditPhysicianModal();
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == id);
            modal.physicianid = physician.Physicianid;
            modal.status = (int?)physician.Status;
            modal.role = (int?)physician.Roleid;
            modal.Firstname = physician.Firstname;
            modal.Lastname = physician.Lastname;
            modal.Email = physician.Email;
            modal.phonenumber = physician.Mobile;
            modal.license = physician.Medicallicense;
            modal.npi = physician.Npinumber;
            modal.synemail = physician.Syncemailaddress;
            modal.Address1 = physician.Address1;
            modal.Address2 = physician.Address2;
            modal.City = physician.City;
            modal.State = (int?)physician.Regionid;
            modal.zipcode = physician.Zip;
            modal.altphone = physician.Altphone;
            modal.BusinessName = physician.Businessname;
            modal.Businesssite = physician.Businesswebsite;
            modal.Adminnotes = physician.Adminnotes;
            modal.photo = physician.Photo;
            modal.sign = physician.Signature;
            modal.Isagreementdoc = physician.Isagreementdoc;
            modal.Isbackgrounddoc = physician.Isbackgrounddoc;
            modal.Iscredentialdoc = physician.Iscredentialdoc;
            modal.Islicensedoc = physician.Islicensedoc;
            modal.Isnondisclosuredoc = physician.Isnondisclosuredoc;
            modal.Username = _context.Aspnetusers.FirstOrDefault(u => u.Id.ToString() == physician.Aspnetuserid).Username;
            modal.regions = _context.Regions.ToList();
            modal.physicianregions = _context.Physicianregions.Include(u => u.Region).Where(u => u.Physicianid == id).Select(u => u.Region).ToHashSet();
            modal.roles = _context.Roles.Where(u => u.Accounttype == 2).ToList();
            return modal;
        }
        public void PhysicianAccInfo(EditPhysicianModal modal, string adminname)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Id.ToString() == physician.Aspnetuserid);
            aspuser.Username = modal.Username;
            physician.Status = (short?)modal.status;
            physician.Roleid = modal.role;
            _context.Aspnetusers.Update(aspuser);
            _context.Physicians.Update(physician);
            physician.Modifieddate = DateTime.Now;
            physician.Modifiedby = adminname;
            _context.SaveChanges();
        }
        public void PhysicianResetPass(EditPhysicianModal modal, string adminname)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            var aspuser = _context.Aspnetusers.FirstOrDefault(u => u.Id.ToString() == physician.Aspnetuserid);
            aspuser.Passwordhash = modal.password;
            _context.Aspnetusers.Update(aspuser);
            physician.Modifieddate = DateTime.Now;
            physician.Modifiedby = adminname;
            _context.SaveChanges();
        }
        public bool PhysicianInfo(EditPhysicianModal modal, string adminname, List<string> chk)
        {
            bool flag = true;
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            var checkmail = _context.Aspnetusers.FirstOrDefault(u => u.Email == modal.Email);
            if (checkmail != null)
            {
                flag = false;
            }
            if (modal.Email == physician.Email)
            {
                flag = true;
            }
            physician.Firstname = modal.Firstname;
            physician.Lastname = modal.Lastname;
            if (flag == true)
            {
                physician.Email = modal.Email;
            }
            physician.Mobile = modal.phonenumber;
            physician.Medicallicense = modal.license;
            physician.Npinumber = modal.npi;
            physician.Syncemailaddress = modal.synemail;
            physician.Modifieddate = DateTime.Now;
            physician.Modifiedby = adminname;
            var phyregion = _context.Physicianregions.Where(u => u.Physicianid == modal.physicianid).ToList();
            foreach (var obj in phyregion)
            {
                _context.Physicianregions.Remove(obj);
            }
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Physicianregion physicianregion = new Physicianregion
                {
                    Physicianid = modal.physicianid,
                    Regionid = s
                };
                _context.Physicianregions.Add(physicianregion);
                _context.SaveChanges();
            }
            _context.Physicians.Update(physician);
            _context.SaveChanges();
            return flag;
        }
        public void PhysicianMailingInfo(EditPhysicianModal modal, string adminname)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            physician.Address1 = modal.Address1;
            physician.Address2 = modal.Address2;
            physician.City = modal.City;
            physician.Regionid = modal.regionid;
            physician.Zip = modal.zipcode;
            physician.Altphone = modal.altphone;
            physician.Modifieddate = DateTime.Now;
            physician.Modifiedby = adminname;
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public void ProviderProfile(EditPhysicianModal modal, string adminname)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            physician.Businessname = modal.BusinessName;
            physician.Businesswebsite = modal.Businesssite;
            physician.Adminnotes = modal.Adminnotes;
            physician.Modifieddate = DateTime.Now;
            physician.Modifiedby = adminname;
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public void EditProviderSign(int physicianid, string base64string)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == physicianid);
            physician.Signature = base64string;
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public void EditProviderPhoto(int physicianid, string base64string)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == physicianid);
            physician.Photo = base64string;
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public void PhyNotification(List<string> chk)
        {
            var phynotificationRemove = _context.Physiciannotifications.ToList();
            foreach (var obj in phynotificationRemove)
            {
                obj.Isnotificationstopped = new BitArray(new[] { false });
                _context.Physiciannotifications.Update(obj);
            }
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                var phynotification = _context.Physiciannotifications.FirstOrDefault(u => u.Physicianid == s);
                phynotification.Isnotificationstopped = new BitArray(new[] { true });
                _context.Physiciannotifications.Update(phynotification);
            }
            _context.SaveChanges();
        }
        public void DeletePhysician(EditPhysicianModal modal)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == modal.physicianid);
            physician.Isdeleted = new BitArray(new[] { true });
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public Task sendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.deeppatel@outlook.com";
            var password = "Deep2292002";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
        public void ContactPhysician(int phyid, string chk, string message)
        {
            if (chk == "email" || chk == "both")
            {
                var email = _context.Physicians.FirstOrDefault(u => u.Physicianid == phyid).Email;
                sendEmail(email, "Contact Physician", message);
            }
        }

        public AccessRoleModal AccessTab()
        {
            AccessRoleModal modal = new AccessRoleModal();
            modal.roles = _context.Roles.Where(U => U.Isdeleted == new BitArray(new[] { false })).ToList();
            return modal;
        }

        public AccessRoleModal CreateRole()
        {
            AccessRoleModal modal = new AccessRoleModal();
            modal.menu = _context.Menus.ToList();
            return modal;
        }
        public void CreateRoleSubmit(AccessRoleModal modal, List<string> chk, string adminname)
        {
            Role role = new Role
            {
                Name = modal.RoleName,
                Accounttype = (short)modal.accountType,
                Createddate = DateTime.Now,
                Createdby = adminname,
                Isdeleted = new BitArray(new[] { false })
            };
            _context.Roles.Add(role);
            _context.SaveChanges();
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Rolemenu rolemenu = new Rolemenu
                {
                    Roleid = role.Roleid,
                    Menuid = s
                };
                _context.Rolemenus.Add(rolemenu);
                _context.SaveChanges();
            }
            _context.SaveChanges();
        }
        public List<Menu> filtermenu(int acctype)
        {
            List<Menu> menu = _context.Menus.Where(u => u.Accounttype == acctype).ToList();
            if (acctype == 3)
            {
                menu = _context.Menus.ToList();
            }
            return menu;
        }
        public AccessRoleModal EditRole(int roleid)
        {
            AccessRoleModal modal = new AccessRoleModal();
            var role = _context.Roles.FirstOrDefault(u => u.Roleid == roleid);
            modal.menu = _context.Menus.Where(u => u.Accounttype == role.Accounttype).ToList();
            modal.selectedmenuid = _context.Rolemenus.Include(r => r.Role).Where(r => r.Roleid == roleid && r.Role.Accounttype == role.Accounttype).Select(r => r.Menu.Menuid).ToList();
            modal.RoleName = role.Name;
            modal.accountType = role.Accounttype;
            modal.roleid = roleid;
            return modal;
        }
        public void EditRoleSubmit(AccessRoleModal modal, List<string> chk, string adminname)
        {
            Role role = _context.Roles.FirstOrDefault(u => u.Roleid == modal.roleid);
            role.Name = modal.RoleName;
            role.Accounttype = (short)modal.accountType;
            role.Modifieddate = DateTime.Now;
            role.Modifiedby = adminname;
            _context.Roles.Update(role);
            _context.SaveChanges();
            var rolemenu = _context.Rolemenus.Where(u => u.Roleid == modal.roleid).ToList();
            foreach (var obj in rolemenu)
            {
                _context.Rolemenus.Remove(obj);
            }
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Rolemenu newrolemenu = new Rolemenu
                {
                    Roleid = role.Roleid,
                    Menuid = s
                };
                _context.Rolemenus.Add(newrolemenu);
                _context.SaveChanges();
            }
            _context.SaveChanges();
        }
        public void DeleteRole(int roleid)
        {
            var role = _context.Roles.FirstOrDefault(u => u.Roleid == roleid);
            role.Isdeleted = new BitArray(new[] { true });
            _context.Roles.Update(role);
            _context.SaveChanges();
        }
        public EditPhysicianModal CreateProviderAcc()
        {
            EditPhysicianModal modal = new EditPhysicianModal();
            modal.regions = _context.Regions.ToList();
            modal.roles = _context.Roles.Where(u => u.Accounttype == 2).ToList();
            return modal;
        }
        public void CreateProviderAccBtn(EditPhysicianModal modal, List<string> chk, string adminname)
        {
            string base64StringWithFileType = "";
            if (modal.Images != null && modal.Images.Length > 0)
            {
                // Convert the image to base64
                using var memorystream = new MemoryStream();
                var file = modal.Images;
                file.CopyTo(memorystream);
                string converted = Convert.ToBase64String(memorystream.ToArray());
                var fileType = file.ContentType;
                base64StringWithFileType = $"data:{fileType};base64,{converted}";

            }
            Aspnetuser aspuser = new Aspnetuser
            {
                Username = modal.Username,
                Passwordhash = modal.password,
                Email = modal.Email,
                Phonenumber = modal.phonenumber,
                Modifieddate = DateTime.Now
            };
            _context.Aspnetusers.Add(aspuser);
            _context.SaveChanges();
            Physician physician = new Physician
            {
                Aspnetuserid = aspuser.Id.ToString(),
                Firstname = modal.Firstname,
                Lastname = modal.Lastname,
                Email = modal.Email,
                Mobile = modal.phonenumber,
                Photo = base64StringWithFileType,
                Adminnotes = modal.Adminnotes,
                Address1 = modal.Address1,
                Address2 = modal.Address2,
                City = modal.City,
                Regionid = modal.regionid,
                Zip = modal.zipcode,
                Altphone = modal.altphone,
                Createdby = adminname,
                Createddate = DateTime.Now,
                Status = (short?)modal.status,
                Businessname = modal.BusinessName,
                Businesswebsite = modal.Businesssite,
                Isdeleted = new BitArray(new[] { false }),
                Roleid = modal.role,
                Npinumber = modal.npi,
                Syncemailaddress = modal.synemail,
            };
            _context.Physicians.Add(physician);
            _context.SaveChanges();
            Physiciannotification physiciannotification = new Physiciannotification
            {
                Physicianid = physician.Physicianid,
                Isnotificationstopped = new BitArray(new[] { false })
            };
            _context.Physiciannotifications.Add(physiciannotification);
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Physicianregion physicianregion = new Physicianregion
                {
                    Physicianid = physician.Physicianid,
                    Regionid = s
                };
                _context.Physicianregions.Add(physicianregion);
                _context.SaveChanges();
            }

        }
        public AdminProfile CreateAdminAcc()
        {
            AdminProfile modal = new AdminProfile();
            modal.roles = _context.Roles.Where(u=>u.Accounttype == 1).ToList();
            modal.regions = _context.Regions.ToList();
            return modal;
        }
        public void CreateAdminAccBtn(AdminProfile modal, List<string> chk, string adminname)
        {
            Aspnetuser aspuser = new Aspnetuser
            {
                Username = modal.Username,
                Passwordhash = modal.ResetPassword,
                Email = modal.email,
                Phonenumber = modal.phonenumber,
                Modifieddate = DateTime.Now
            };
            _context.Aspnetusers.Add(aspuser);
            _context.SaveChanges();
            Admin admin = new Admin
            {
                Aspnetuserid = aspuser.Id.ToString(),
                Firstname = modal.firstname,
                Lastname = modal.lastname,
                Email = modal.email,
                Mobile = modal.phonenumber,
                Address1 = modal.address1,
                Address2 = modal.address2,
                City = modal.city,
                Regionid = modal.regionid,
                Zip = modal.zipcode,
                Altphone = modal.altphone,
                Createddate = DateTime.Now,
                Createdby = adminname,
                Roleid = modal.roleid,
                Isdeleted = new BitArray(new[] { false })
            };
            _context.Admins.Add(admin);
            _context.SaveChanges();
            foreach (var obj in chk)
            {
                var s = Int32.Parse(obj);
                Adminregion newAdminRegion = new Adminregion
                {
                    Adminid = admin.Adminid,
                    Regionid = s,
                };
                _context.Adminregions.Add(newAdminRegion);
                _context.SaveChanges();
            }
            Aspnetuserrole asprole = new Aspnetuserrole
            {
                Userid = aspuser.Id.ToString(),
                Roleid = "1"
            };
            _context.SaveChanges();
        }
        public void phyuploadDoc(int physicianid, string doctype)
        {
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == physicianid);
            if (doctype == "ICA")
            {
                physician.Isagreementdoc = new BitArray(new[] { true });
            }
            else if(doctype == "HIPAA")
            {
                physician.Iscredentialdoc = new BitArray(new[] { true });
            }
            else if(doctype == "BGCheck")
            {
                physician.Isbackgrounddoc = new BitArray(new[] { true });
            }
            else if(doctype == "NDDoc")
            {
                physician.Isnondisclosuredoc = new BitArray(new[] { true });
            }
            else if(doctype == "LDDoc")
            {
                physician.Islicensedoc = new BitArray(new[] { true });
            }
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
    }
}
