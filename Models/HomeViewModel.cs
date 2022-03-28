namespace WETT.Models
{
	public class HomeViewModel
	{
		private string _userId;
		public string UserId
		{
			get
			{
				if (_userId == null)
				{
					try
					{
						_userId = System.Security.Claims.ClaimsPrincipal.Current.Identity.Name;

					}
					catch
					{
						_userId = "unknown user";
					}
				}
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}
	}
}
