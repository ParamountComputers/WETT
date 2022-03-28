namespace WETT.Models
{
    public class HomeViewModel
    {
        private string _userId;
        public string userId {
        get
            {
                if (_userId == null) {
                    try
                    {
                        _userId = System.Security.Claims.ClaimsPrincipal.Current.Identity.Name;
                    }
                    catch
                    {
                        _userId = "unkown user";
                    }
                }
                return _userId;
            }
        }
    }
}
