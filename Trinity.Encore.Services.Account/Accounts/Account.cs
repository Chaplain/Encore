using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Security.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Services.Account.Bans;
using Trinity.Encore.Services.Account.Database;

namespace Trinity.Encore.Services.Account.Accounts
{
    public sealed class Account
    {
        public Account(AccountRecord record)
        {
            Contract.Requires(record != null);

            Record = record;
        }

        /// <summary>
        /// Deletes the Account from the backing storage. This object is considered invalid once
        /// this method has been executed.
        /// </summary>
        internal void Delete()
        {
            Record.Delete();
        }

        public AccountData Serialize()
        {
            Contract.Ensures(Contract.Result<AccountData>() != null);

            return new AccountData
            {
                Id = Id,
                Name = Name,
                Password = Password,
                BoxLevel = BoxLevel,
                Locale = Locale,
                LastLogin = LastLogin,
                LastIP = LastIP,
                RecruiterId = Record.Recruiter != null ? Record.Recruiter.Id : 0, // TODO: Use Account.Recruiter.
            };
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Record != null);
        }

        /// <summary>
        /// Gets the underlying record of this Account. Should not be manipulated
        /// directly.
        /// </summary>
        public AccountRecord Record { get; private set; }

        public long Id
        {
            get { return Record.Id; }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                Contract.Ensures(Contract.Result<string>().Length >= AccountManager.MinNameLength);
                Contract.Ensures(Contract.Result<string>().Length <= AccountManager.MaxNameLength);

                var name = Record.Name;
                Contract.Assume(!string.IsNullOrEmpty(name));
                Contract.Assume(name.Length >= AccountManager.MinNameLength);
                Contract.Assume(name.Length <= AccountManager.MaxNameLength);
                return name;
            }
        }

        public string EmailAddress
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                var email = Record.EmailAddress;
                Contract.Assume(!string.IsNullOrEmpty(email));
                return email;
            }
            set
            {
                Contract.Requires(!string.IsNullOrEmpty(value));

                Record.EmailAddress = value;
                Record.Update();
            }
        }

        public void ChangePassword(string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(password.Length >= AccountManager.MinPasswordLength);
            Contract.Requires(password.Length <= AccountManager.MaxPasswordLength);

            Password = AccountManager.CreatePassword(Name, password);
        }

        public Password Password
        {
            get
            {
                Contract.Ensures(Contract.Result<Password>() != null);

                Contract.Assume(Record.SHA1Password != null);
                Contract.Assume(Record.SHA1Password.Length == Password.SHA1Length);
                Contract.Assume(Record.SHA256Password != null);
                Contract.Assume(Record.SHA256Password.Length == Password.SHA256Length);
                return new Password(Record.SHA1Password, Record.SHA256Password);
            }
            set
            {
                Contract.Requires(value != null);

                Record.SHA1Password = value.SHA1Password.GetBytes();
                Record.SHA256Password = value.SHA256Password.GetBytes();
                Record.Update();
            }
        }

        public ClientBoxLevel BoxLevel
        {
            get { return Record.BoxLevel; }
            set
            {
                Record.BoxLevel = value;
                Record.Update();
            }
        }

        public ClientLocale Locale
        {
            get { return Record.Locale; }
            set
            {
                Record.Locale = value;
                Record.Update();
            }
        }

        public DateTime? LastLogin
        {
            get { return Record.LastLogin; }
            set
            {
                Record.LastLogin = value;
                Record.Update();
            }
        }

        public IPAddress LastIP
        {
            get
            {
                var ipBytes = Record.LastIP;
                return ipBytes != null ? new IPAddress(ipBytes) : null;
            }
            set
            {
                Record.LastIP = value != null ? value.GetAddressBytes() : null;
                Record.Update();
            }
        }

        public Account Recruiter
        {
            get
            {
                var recruiter = Record.Recruiter;
                return recruiter != null ? AccountManager.Instance.FindAccount(x => x.Id == recruiter.Id) : null;
            }
            set
            {
                Record.Recruiter = value != null ? value.Record : null;
                Record.Update();
            }
        }

        public AccountBan Ban
        {
            get { return BanManager.Instance.FindAccountBan(x => x.Account.Id == Id); }
        }
    }
}
