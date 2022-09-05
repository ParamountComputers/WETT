namespace WETT.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Net.Mail;
	using Telerik.Reporting.Services;
	using Telerik.Reporting.Services.AspNetCore;

	[Route("api/reports")]
	public class ReportsController : ReportsControllerBase
	{
		public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
			: base(reportServiceConfiguration)
		{
		}
	}
}
