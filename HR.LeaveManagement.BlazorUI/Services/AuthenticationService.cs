using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Providers;
using HR.LeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace HR.LeaveManagement.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(IClient client, ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider) : 
            base(client, localStorageService)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            try
            {
                AuthRequest authenticationRequest = new AuthRequest() { Email = email, Password = password };

                var authenticationResponse = await _client.LoginAsync(authenticationRequest);

                if (authenticationResponse.Token != string.Empty)
                {
                    await _localStorage.SetItemAsync("token", authenticationResponse.Token);

                    // Set claims in Blazor and login state
                    await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedIn();
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
            
        }

        public async Task Logout()
        {
            //remove claims in Blazor and onvalidate login state
            await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            try
            {
                RegistrationRequest registrationRequest = new RegistrationRequest() { FirstName = firstName,
                    LastName = lastName, Email = email, Password = password, UserName = userName };
                var response = await _client.RegiterAsync(registrationRequest);

                if(!string.IsNullOrEmpty(response.UserID))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
