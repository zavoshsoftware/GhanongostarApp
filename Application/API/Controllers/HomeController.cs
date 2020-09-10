using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Helper;
using Models;
using Models.Input;

namespace API.Controllers
{
    public class HomeController : Infrastructure.BaseControllerWithUnitOfWork
    {
        StatusManagement status = new StatusManagement();
        PageCounter pageCounter = new PageCounter();

        [Route("")]
        public string GetTest()
        {

            User user = UnitOfWork.UserRepository.Get().FirstOrDefault();
            return user?.FullName;


        }

        [Authorize]
        [Route("home/get")]
        [HttpPost]
        public HomeViewModel Get(HomeInputViewModel input)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    homeViewModel.Result = null;
                    homeViewModel.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return homeViewModel;
                }
                if (!(String.IsNullOrEmpty(input.Version)))
                {
                    user.VersionNumber = input.Version;
                }
                if (!(String.IsNullOrEmpty(input.OsType)))
                {
                    user.OsType = input.OsType;
                }
                if (!(String.IsNullOrEmpty(input.Version)) || !(String.IsNullOrEmpty(input.OsType)))
                {
                    UnitOfWork.UserRepository.Update(user);
                    UnitOfWork.Save();
                }

                homeViewModel = GetHomeResult(user);

                pageCounter.Count("home", null);
            }
            catch (Exception e)
            {
                homeViewModel = ReturnReject();
            }
            return homeViewModel;
        }

        [Authorize]
        [Route("home/calculate")]
        [HttpPost]
        public CalculateViewModel GetSallary(CalculateInputViewModel input)
        {
            CalculateViewModel result = new CalculateViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                result.Result = GetCalculateResult(input);
                result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);

                pageCounter.Count("calculate", null);

            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
            }
            return result;
        }


        public CalculateItemsViewModel GetCalculateResult(CalculateInputViewModel input)
        {
            CalculateItemsViewModel res = new CalculateItemsViewModel();

            res.Eydi = GetEidiResult(input);
            res.Sanavat = GetSanavatResult(input);
            res.Morakhasi = GetMorkhasiResult(input);
            res.Bon = GetBonResult(input);
            res.Maskan = GetMaskanResult(input);
            res.Olad = GetOladResult(input);
            res.PayeSanavat = GetPayeSanavatResult(input);
            res.SanavatDescription = UnitOfWork.TextRepository.Get(current => current.Title.ToLower() == "sanavat").FirstOrDefault().Body;
            res.MorkhasiDescription = UnitOfWork.TextRepository.Get(current => current.Title.ToLower() == "morkhasi").FirstOrDefault().Body;
            return res;
        }

        public List<KeyValueViewModel> GetEidiResult(CalculateInputViewModel input)
        {
            List<KeyValueViewModel> res = new List<KeyValueViewModel>();
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);

            for (int i = startYear; i < finishYear; i++)
            {
                res.Add(new KeyValueViewModel()
                {
                    Key = i.ToString(),
                    Value = (RerurnMinSalary(i) * 2).ToString()
                });
            }
            int doubleSalary = int.Parse(input.Sallary) * 2;
            int minSalary = RerurnMinSalary(finishYear) * 2;
            int maxSalary = RerurnMinSalary(finishYear) * 3;

            int lastYearEidi = 0;
            int lastYearMonthEidi = 0;

            if (doubleSalary < minSalary)
            {
                lastYearEidi = minSalary;
            }
            else if (doubleSalary > maxSalary)
            {
                lastYearEidi = maxSalary;
            }
            else
            {
                lastYearEidi = minSalary;
            }
            lastYearMonthEidi = (lastYearEidi / 12) * int.Parse(input.FinishMonth);
            res.Add(new KeyValueViewModel()
            {
                Key = input.FinishYear,
                Value = lastYearMonthEidi.ToString()
            });

            return res;
        }

        public List<KeyValueViewModel> GetPayeSanavatResult(CalculateInputViewModel input)
        {
            List<KeyValueViewModel> result = new List<KeyValueViewModel>();
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            for (int i = startYear; i <= finishYear; i++)
            {
                result.Add(new KeyValueViewModel()
                {
                    Key = i.ToString(),
                    Value = (ReturnSanavat(i) * 30).ToString()
                });
            }
            return result;

        }

        public List<KeyValueViewModel> GetOladResult(CalculateInputViewModel input)
        {
            List<KeyValueViewModel> result = new List<KeyValueViewModel>();
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            for (int i = startYear; i <= finishYear; i++)
            {
                result.Add(new KeyValueViewModel()
                {
                    Key = i.ToString(),
                    Value = RerurnOlad(i).ToString()
                });
            }
            return result;
        }
        public List<KeyValueViewModel> GetBonResult(CalculateInputViewModel input)
        {
            List<KeyValueViewModel> result = new List<KeyValueViewModel>();
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            for (int i = startYear; i <= finishYear; i++)
            {
                result.Add(new KeyValueViewModel()
                {
                    Key = i.ToString(),
                    Value = RerurnBon(i).ToString()
                });
            }
            return result;
        }
        public List<KeyValueViewModel> GetMaskanResult(CalculateInputViewModel input)
        {
            List<KeyValueViewModel> result = new List<KeyValueViewModel>();
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            for (int i = startYear; i <= finishYear; i++)
            {
                result.Add(new KeyValueViewModel()
                {
                    Key = i.ToString(),
                    Value = RerurnMaskan(i).ToString()
                });
            }
            return result;
        }

        public string GetSanavatResult(CalculateInputViewModel input)
        {
            string sanavatAmout;

            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            int startMonth = int.Parse(input.StartMonth);
            int finishMonth = int.Parse(input.FinishMonth);
            int sallary = int.Parse(input.Sallary);

            int year = finishYear - startYear;

            int monthWork = 12;

            if (year > 1)
            {
                int yearWork = (finishYear - 1) - (startYear + 1) + 1;

                monthWork = (yearWork * 12) + (12 - startMonth) + finishMonth;
            }
            else if (year == 1)
                monthWork = (12 - startMonth) + finishMonth;
            else if (year == 0)
                monthWork = finishMonth - startMonth + 1;

            int test = (sallary / 12);
            decimal test2 = test * monthWork;
            decimal i = Math.Round(test2, mode: MidpointRounding.AwayFromZero);
            sanavatAmout = ((sallary / 12) * monthWork).ToString();

            return sanavatAmout;
        }
        public string GetMorkhasiResult(CalculateInputViewModel input)
        {
            string result;
            int startYear = int.Parse(input.StartYear);
            int finishYear = int.Parse(input.FinishYear);
            int startMonth = int.Parse(input.StartMonth);
            int finishMonth = int.Parse(input.FinishMonth);
            int sallary = int.Parse(input.Sallary);

            int yearWork = (finishYear - 1) - (startYear + 1) + 1;
            int monthWork = (yearWork * 12) + (12 - startMonth) + finishMonth;

            if (monthWork < 12)
                result = "0";
            else
            {
                result = (((sallary / 30) * 9) * (monthWork / 12)).ToString();
            }
            return result;
        }
        public HomeViewModel GetHomeResult(User user)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Result = GetHomeItems(user);
            homeViewModel.Status = status.ReturnStatus(0, "اطلاعات صفحه اصلی اپلیکیشن", true);

            return homeViewModel;
        }

        public HomeItems GetHomeItems(User user)
        {
            HomeItems home = new HomeItems()
            {
                Images = GetHeaderImages(),
                MessageCount = "1",
            };
            return home;
        }

        public List<string> GetHeaderImages()
        {
            string baseUrl = WebConfigurationManager.AppSettings["BaseUrl"];

            List<string> images = new List<string>();
            for (int i = 1; i < 11; i++)
            {
                if (i != 7)
                {
                    string fileName = "Images/Home/" + i + ".png";
                    images.Add(baseUrl + fileName);
                }
                else
                {
                    string fileName = "Images/Home/20.png";
                    images.Add(baseUrl + fileName);
                }
            }

            return images;
        }
        public HomeViewModel ReturnReject()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Result = null;
            homeViewModel.Status = status.ReturnStatus(100, "خطا در بازیابی اطلاعات", false);

            return homeViewModel;
        }
        public string GetRequestHeader()
        {
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("Authorization"))
            {
                string token = headers.GetValues("Authorization").First();
                return token;
            }
            return String.Empty;
        }

        public int RerurnMinSalary(int year)
        {
            int amount = 0;
            switch (year)
            {
                case 1380:
                    {
                        amount = 56790;
                        break;
                    }
                case 1381:
                    {
                        amount = 69846;
                        break;
                    }
                case 1382:
                    {
                        amount = 85338;
                        break;
                    }
                case 1383:
                    {
                        amount = 106602;
                        break;
                    }
                case 1384:
                    {
                        amount = 122592;
                        break;
                    }
                case 1385:
                    {
                        amount = 150000;
                        break;
                    }
                case 1386:
                    {
                        amount = 183000;
                        break;
                    }
                case 1387:
                    {
                        amount = 219600;
                        break;
                    }
                case 1388:
                    {
                        amount = 263520;
                        break;
                    }
                case 1389:
                    {
                        amount = 303000;
                        break;
                    }
                case 1390:
                    {
                        amount = 330300;
                        break;
                    }
                case 1391:
                    {
                        amount = 389700;
                        break;
                    }
                case 1392:
                    {
                        amount = 487125;
                        break;
                    }
                case 1393:
                    {
                        amount = 608906;
                        break;
                    }
                case 1394:
                    {
                        amount = 712425;
                        break;
                    }
                case 1395:
                    {
                        amount = 812166;
                        break;
                    }
                case 1396:
                    {
                        amount = 929931;
                        break;
                    }
                case 1397:
                    {
                        amount = 1111269;
                        break;
                    }
                case 1398:
                    {
                        amount = 1516882;
                        break;
                    }
            }

            return amount;
        }
        public int RerurnMaxSalary(int year)
        {
            int amount = 0;
            switch (year)
            {
                case 1380:
                    {
                        amount = 397530;
                        break;
                    }
                case 1381:
                    {
                        amount = 488922;
                        break;
                    }
                case 1382:
                    {
                        amount = 597366;
                        break;
                    }
                case 1383:
                    {
                        amount = 746214;
                        break;
                    }
                case 1384:
                    {
                        amount = 858144;
                        break;
                    }
                case 1385:
                    {
                        amount = 1050000;
                        break;
                    }
                case 1386:
                    {
                        amount = 1281000;
                        break;
                    }
                case 1387:
                    {
                        amount = 1537200;
                        break;
                    }
                case 1388:
                    {
                        amount = 1844640;
                        break;
                    }
                case 1389:
                    {
                        amount = 2121000;
                        break;
                    }
                case 1390:
                    {
                        amount = 2312100;
                        break;
                    }
                case 1391:
                    {
                        amount = 2727900;
                        break;
                    }
                case 1392:
                    {
                        amount = 3409875;
                        break;
                    }
                case 1393:
                    {
                        amount = 4262342;
                        break;
                    }
                case 1394:
                    {
                        amount = 4986975;
                        break;
                    }
                case 1395:
                    {
                        amount = 5685162;
                        break;
                    }
                case 1396:
                    {
                        amount = 6509517;
                        break;
                    }
                case 1397:
                    {
                        amount = 7778883;
                        break;
                    }
                case 1398:
                    {
                        amount = 1516882;
                        break;
                    }
            }

            return amount;
        }
        public int ReturnSanavat(int year)
        {
            int amount = 0;
            switch (year)
            {
                case 1380:
                    {
                        amount = 47;
                        break;
                    }
                case 1381:
                    {
                        amount = 58;
                        break;
                    }
                case 1382:
                    {
                        amount = 71;
                        break;
                    }
                case 1383:
                    {
                        amount = 90;
                        break;
                    }
                case 1384:
                    {
                        amount = 102;
                        break;
                    }
                case 1385:
                    {
                        amount = 125;
                        break;
                    }
                case 1386:
                    {
                        amount = 125;
                        break;
                    }
                case 1387:
                    {
                        amount = 125;
                        break;
                    }
                case 1388:
                    {
                        amount = 125;
                        break;
                    }
                case 1389:
                    {
                        amount = 200;
                        break;
                    }
                case 1390:
                    {
                        amount = 200;
                        break;
                    }
                case 1391:
                    {
                        amount = 250;
                        break;
                    }
                case 1392:
                    {
                        amount = 300;
                        break;
                    }
                case 1393:
                    {
                        amount = 500;
                        break;
                    }
                case 1394:
                    {
                        amount = 1000;
                        break;
                    }
                case 1395:
                    {
                        amount = 1000;
                        break;
                    }
                case 1396:
                    {
                        amount = 1700;
                        break;
                    }
                case 1397:
                    {
                        amount = 1700;
                        break;
                    }
                case 1398:
                    {
                        amount = 2333;
                        break;
                    }
            }

            return amount;
        }
        public int RerurnOlad(int year)
        {
            int amount = 0;
            switch (year)
            {
                case 1380:
                    {
                        amount = 5679;
                        break;
                    }
                case 1381:
                    {
                        amount = 6985;
                        break;
                    }
                case 1382:
                    {
                        amount = 8534;
                        break;
                    }
                case 1383:
                    {
                        amount = 10660;
                        break;
                    }
                case 1384:
                    {
                        amount = 12259;
                        break;
                    }
                case 1385:
                    {
                        amount = 15000;
                        break;
                    }
                case 1386:
                    {
                        amount = 18300;
                        break;
                    }
                case 1387:
                    {
                        amount = 21960;
                        break;
                    }
                case 1388:
                    {
                        amount = 26352;
                        break;
                    }
                case 1389:
                    {
                        amount = 30300;
                        break;
                    }
                case 1390:
                    {
                        amount = 33030;
                        break;
                    }
                case 1391:
                    {
                        amount = 38970;
                        break;
                    }
                case 1392:
                    {
                        amount = 48712;
                        break;
                    }
                case 1393:
                    {
                        amount = 60888;
                        break;
                    }
                case 1394:
                    {
                        amount = 71242;
                        break;
                    }
                case 1395:
                    {
                        amount = 81216;
                        break;
                    }
                case 1396:
                    {
                        amount = 92991;
                        break;
                    }
                case 1397:
                    {
                        amount = 111126;
                        break;
                    }
                case 1398:
                    {
                        amount = 151688;
                        break;
                    }
            }

            return amount;
        }
        public int RerurnBon(int year)
        {
            int amount = 0;
            switch (year)
            {
                //case 1380:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1381:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1382:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1383:
                //    {
                //        amount = 2000;
                //        break;
                //    }
                //case 1384:
                //    {
                //        amount = 4000;
                //        break;
                //    }
                //case 1385:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1386:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1387:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1388:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1389:
                //    {
                //        amount = 20000;
                //        break;
                //    }
                case 1390:
                    {
                        amount = 28000;
                        break;
                    }
                case 1391:
                    {
                        amount = 35000;
                        break;
                    }
                case 1392:
                    {
                        amount = 35000;
                        break;
                    }
                case 1393:
                    {
                        amount = 80000;
                        break;
                    }
                case 1394:
                    {
                        amount = 110000;
                        break;
                    }
                case 1395:
                    {
                        amount = 110000;
                        break;
                    }
                case 1396:
                    {
                        amount = 110000;
                        break;
                    }
                case 1397:
                    {
                        amount = 110000;
                        break;
                    }
                case 1398:
                    {
                        amount = 190000;
                        break;
                    }
            }

            return amount;
        }
        public int RerurnMaskan(int year)
        {
            int amount = 0;
            switch (year)
            {
                //case 1380:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1381:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1382:
                //    {
                //        amount = 1000;
                //        break;
                //    }
                //case 1383:
                //    {
                //        amount = 2000;
                //        break;
                //    }
                //case 1384:
                //    {
                //        amount = 4000;
                //        break;
                //    }
                //case 1385:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1386:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1387:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1388:
                //    {
                //        amount = 10000;
                //        break;
                //    }
                //case 1389:
                //    {
                //        amount = 20000;
                //        break;
                //    }
                case 1390:
                    {
                        amount = 10000;
                        break;
                    }
                case 1391:
                    {
                        amount = 10000;
                        break;
                    }
                case 1392:
                    {
                        amount = 20000;
                        break;
                    }
                case 1393:
                    {
                        amount = 20000;
                        break;
                    }
                case 1394:
                    {
                        amount = 40000;
                        break;
                    }
                case 1395:
                    {
                        amount = 20000;
                        break;
                    }
                case 1396:
                    {
                        amount = 40000;
                        break;
                    }
                case 1397:
                    {
                        amount = 40000;
                        break;
                    }
                case 1398:
                    {
                        amount = 100000;
                        break;
                    }
            }

            return amount;
        }
    }
}
