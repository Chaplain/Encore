using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Services;
using Trinity.Encore.Framework.Network;

namespace Trinity.Encore.Framework.Services.Account
{
    [ContractClass(typeof(AccountServiceContracts))]
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAccountService
    {
        AccountData GetAccount(string username);

        void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale = ClientLocale.English,
            ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm);

        AccountBanData GetAccountBan(string username);

        void CreateAccountBan(string accountName, string notes = null, DateTime? expiry = null);

        IPBanData GetIPBan(IPAddress address);

        void CreateIPBan(IPAddress address, string notes = null, DateTime? expiry = null);
        
        IPRangeBanData GetIPRangeBan(IPAddress address);

        void CreateIPRangeBan(IPAddressRange range, string notes = null, DateTime? expiry = null);
    }

    [ContractClassFor(typeof(IAccountService))]
    public abstract class AccountServiceContracts : IAccountService
    {
        public AccountData GetAccount(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            return null;
        }

        public void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale, ClientBoxLevel boxLevel)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(emailAddress));
        }

        public AccountBanData GetAccountBan(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            return null;
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
        }

        public IPBanData GetIPBan(IPAddress address)
        {
            Contract.Requires(address != null);

            return null;
        }

        public void CreateIPBan(IPAddress address, string notes, DateTime? expiry)
        {
            Contract.Requires(address != null);
        }

        public IPRangeBanData GetIPRangeBan(IPAddress address)
        {
            Contract.Requires(address != null);

            return null;
        }

        public void CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            Contract.Requires(range != null);
        }
    }
}
