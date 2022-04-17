using System.Collections.Generic;
using System.Security.Claims;

namespace WETT.Models
{
	public class AuthViewModel
	{
		private IEnumerable<Claim> _claims;
		public string IsAuthenticated { get; set; }
		public string AuthType { get; set; }
		public string UserId { get; set; }
		public string Name { get; set; }

		public IEnumerable<Claim> Claims
		{
			get
			{
				if (_claims == null) _claims = new List<Claim>();
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
