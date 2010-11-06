using System.Security.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography.SRP;

namespace Trinity.Encore.Framework.Game.Cryptography
{
    public sealed class SC2AuthParameters : SRPParameters
    {
        public const int KeySize = 128;

        public SC2AuthParameters(SRPVersion version, bool caseSensitive)
            : base(version, caseSensitive)
        {
        }

        protected override void SetupParameters()
        {
            Hash = new SHA256Managed();
            Generator = new BigInteger(2);
            Modulus = new BigInteger("86A7F6DEEB306CE519770FE37D556F29944132554DED0BD68205E27F3231FEF5A10108238A3150C59CAF7B0B6478691C13A6ACF5E1B5ADAFD4A943D4A21A142B800E8A55F8BFBAC700EB77A7235EE5A609E350EA9FC19F10D921C2FA832E4461B7125D38D254A0BE873DFC27858ACB3F8B9F258461E4373BC3A6C2A9634324AB");
        }

        public static SC2AuthParameters Default
        {
            get { return new SC2AuthParameters(SRPVersion.SRP6A, true); }
        }

        public override int KeyLength
        {
            get { return KeySize; }
        }
    }
}