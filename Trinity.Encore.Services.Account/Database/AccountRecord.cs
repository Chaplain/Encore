using System;
using System.Diagnostics.Contracts;
using System.Net;
using FluentNHibernate.Mapping;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Persistence;
using Trinity.Encore.Framework.Persistence.Mapping;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Services.Account.Accounts;
using Trinity.Encore.Services.Account.Database.Implementation;

namespace Trinity.Encore.Services.Account.Database
{
    public class AccountRecord : AccountDatabaseRecord
    {
        /// <summary>
        /// Constructs a new AccountRecord object.
        /// 
        /// This should be used only by the underlying database layer.
        /// </summary>
        protected AccountRecord()
        {
        }

        /// <summary>
        /// Constructs a new AccountRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="sha1"></param>
        /// <param name="sha256"></param>
        /// <param name="boxLevel"></param>
        /// <param name="locale"></param>
        public AccountRecord(string name, string email, byte[] sha1, byte[] sha256, ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm,
            ClientLocale locale = ClientLocale.English)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(name.Length >= AccountManager.MinNameLength);
            Contract.Requires(name.Length <= AccountManager.MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(email));
            Contract.Requires(sha1 != null);
            Contract.Requires(sha1.Length == Password.SHA1Length);
            Contract.Requires(sha256 != null);
            Contract.Requires(sha256.Length == Password.SHA256Length);

            Name = name;
            EmailAddress = email;
            SHA1Password = sha1;
            SHA256Password = sha256;
            BoxLevel = boxLevel;
            Locale = locale;
        }

        public virtual long Id { get; protected set; }

        public virtual string Name { get; protected /*private*/ set; }

        public virtual string EmailAddress { get; set; }

        public virtual byte[] SHA1Password { get; set; }

        public virtual byte[] SHA256Password { get; set; }
        
        public virtual ClientBoxLevel BoxLevel { get; set; }

        public virtual ClientLocale Locale { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual byte[] LastIP { get; set; }

        public virtual AccountRecord Recruiter { get; set; }

        public virtual AccountBanRecord Ban { get; set; }
    }
    
    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id).Not.Nullable().GeneratedBy.Increment().Unique();
            Map(c => c.Name).Not.Nullable().ReadOnly().Length(AccountManager.MaxNameLength);
            Map(c => c.EmailAddress).Not.Nullable().Update();
            Map(c => c.SHA1Password).Not.Nullable().Update().Length(Password.SHA1Length);
            Map(c => c.SHA256Password).Not.Nullable().Update().Length(Password.SHA256Length);
            Map(c => c.BoxLevel).Not.Nullable().Update();
            Map(c => c.Locale).Not.Nullable().Update();
            Map(c => c.LastLogin).Nullable().Update();
            Map(c => c.LastIP).Nullable().Update();
            References(x => x.Recruiter).Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            HasOne(c => c.Ban).Cascade.All().LazyLoad(Laziness.Proxy);
        }
    }
 }
