using LoanProcessingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoanProcessingSystem.Controllers
{
    public class LoginRegisterController : Controller
    {
        // GET: LoginRegister
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserRegister userRegister)
        {
            bool Status = false;
            string message = "";
            // Model Validation
            if (ModelState.IsValid)
            {
                // //Email is already Exist
                var isExist = IsEmailExist(userRegister.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("Email Exist", "Email Address already exist");
                    return View(userRegister);
                }
                #region Password Hashing
                userRegister.Password = Crypto.Hash(userRegister.Password);
                userRegister.ConfirmPassword = Crypto.Hash(userRegister.ConfirmPassword);
                #endregion
                #region Save to Database
                using (MortgageDbEntities dc = new MortgageDbEntities())
                {
                    dc.UserRegisters.Add(userRegister);
                    dc.SaveChanges();
                    // SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration Successfully done";
                    Status = true;
                }
                #endregion
                SendEmail(userRegister.EmailId);

            }
            else

            {
                message = "InValid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            //Generate Activation Code

            //Password Hashing
            //Save data to database
            //Send Email to user


            
            return RedirectToAction("LogIn", "LoginRegister");

        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                var v = dc.UserRegisters.Where(a => a.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLogin login, string ReturnUrl)
        {
            string message = "";
            ViewBag.Message = message;
            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                var v = dc.UserRegisters.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
                if (v != null)
                {

                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememerMe ? 525600 : 1;
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememerMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["Emailid"] = login.EmailId;
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid Credential provided";
                    }
                }
                else
                {
                    message = "Invalid Credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        [HttpGet]
        public ActionResult ManagerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManagerLogin(HighAuthorityLogin managerlogin)
        {
            string message = "";
            ViewBag.Message = message;
            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                var v = dc.ManagerDetails.Where(a => a.EmailId == managerlogin.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare((managerlogin.Password), v.Password) == 0)
                    {
                        Session["id"] = v.ManagerId;
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        message = "Invalid Credential provided";
                    }
                }
                else
                {
                    message = "Invalid Credential provided";
                }

            }
            ViewBag.Message = message;
            return View();

        }
        [HttpGet]
        public ActionResult InspectorLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InspectorLogin(HighAuthorityLogin inspectorlogin)
        {
            string message = "";
            ViewBag.Message = message;
            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                var v = dc.InspectorDetails.Where(a => a.EmailId == inspectorlogin.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare((inspectorlogin.Password), v.Password) == 0)
                    {
                        Session["id"] = v.InspectorId;
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        message = "Invalid Credential provided";
                    }
                }
                else
                {
                    message = "Invalid Credential provided";
                }

            }
            ViewBag.Message = message;
            return View();

        }
        public ActionResult Logout()
        {
            string Id = (string)Session["Id"];
            Session.Abandon();
            return RedirectToAction("LogIn", "LoginRegister");
        }
        [NonAction]
        public void SendEmail(string EmailId)
        {
            var from = new MailAddress("agrawalsanskriti00@gmail.com", "Santander UK ");
            var to = new MailAddress(EmailId);
            var frompw = "Sanskriti7691";
            string sub = "your account is successfully created";
            string body = "<br/><br/>We are excited to tell you that your account is successfully created on <strong>Santander UK</strong>" +
                       "<br/><br/> Congratulations!!";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, frompw)


            };
            using (var message = new MailMessage(from, to))
            {
                message.Subject = sub;
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.Send(message);

            }
        }
    }
}