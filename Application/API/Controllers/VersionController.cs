using API.Models;
using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models.Input;

namespace API.Controllers
{
    public class VersionController : Infrastructure.BaseControllerWithUnitOfWork
    {
        StatusManagement status = new StatusManagement();

        [HttpPost]
        [Route("LatestVersion")]
        public VersionHistoryViewModel GetLatestVersion(VersionInputViewModel input)
        {
            VersionHistoryViewModel versionHistoryViewModel = new VersionHistoryViewModel();
            try
            {
                versionHistoryViewModel.Result = ReturnVersion(input.OsType);
                versionHistoryViewModel.Status = status.ReturnStatus(0, Resources.Messages.Success, true);
                return versionHistoryViewModel;
            }
            catch
            {
                versionHistoryViewModel.Result = null;
                versionHistoryViewModel.Status = status.ReturnStatus(100, Resources.Messages.Failed, false);
                return versionHistoryViewModel;
            }
        }

     
        public VersionItems ReturnVersion(string osType)
        {
            VersionItems versionItem = new VersionItems();

            VersionHistory versionHistory = UnitOfWork.VersionHistoryRepository
                .Get(current => current.IsActive == true&&current.Os.ToLower()==osType)
                .OrderByDescending(current => current.VersionNumber).FirstOrDefault();

            if (versionHistory != null)
            {
                versionItem.VersionNumber = versionHistory.VersionNumber;
           
                versionItem.Link = "https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en";

                versionItem.IsNeccessary = versionHistory.IsNeccessary;

                versionItem.LatestStableVersion = versionHistory.LatestStableVersion;

                versionItem.IsBeta = versionHistory.IsBeta;
            }
            return versionItem;
        }
    }
}