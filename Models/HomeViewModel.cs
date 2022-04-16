using System.Collections.Generic;

namespace WETT.Models
{
	public class HomeViewModel
	{
		private ICollection<string> _claims;
		public string IsAuthenticated { get; set; }
		public string AuthType { get; set; }
		public string UserId { get; set; }
		public string Name { get; set; }

		public ICollection<string> Claims
		{
			get
			{
				if( _claims == null ) _claims = new List<string>();
				return _claims;
			}
			set
			{
				_claims = value;
			}
		}
		public string CompileMode { get; set; }	
	}
}
