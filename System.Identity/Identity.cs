using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Identity
{

    public class Group : Identity
    {
        public readonly ObservableCollection<Identity> Members = new ObservableCollection<Identity>();

        protected override string FormatName()
        {
            throw new NotImplementedException();
        }

        protected override void ParseFormattedName(string formattedName)
        {
            throw new NotImplementedException();
        }
    }
    
    public class Individual : Identity
    {
        protected override string FormatName()
        {
            return this.Name;
        }

        protected override void ParseFormattedName(string formattedName)
        {
            this.Name = formattedName;
        }
    }

    public class Organization : Identity
    {
        protected override string FormatName()
        {
            throw new NotImplementedException();
        }

        protected override void ParseFormattedName(string formattedName)
        {
            throw new NotImplementedException();
        }
    }

    public class Localization : Identity
    {
        protected override string FormatName()
        {
            throw new NotImplementedException();
        }

        protected override void ParseFormattedName(string formattedName)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Identity : INotifyPropertyChanged
    {
        #region General properties
        public const string VALUE_PARAMETER_SEPARATOR = ":";
        public const string IDENTIFIER = "VCARD";
        public const string NEW_LINE = "\r\n";

        public const string BEGIN_HEADER = "BEGIN";
        public const string END_FOOTER = "END";

        public const string SOURCE_PROPERTIES = "SOURCE";
        private Uri source;
        /// <summary>
        /// To identify the source of directory information contained in the content type
        /// </summary>
        /// <remarks>
        /// The SOURCE property is used to provide the means by which applications knowledgable in the given directory service protocol can obtain additional or more up-to-date information from the directory service
        /// It contains a URI as defined in [RFC3986] and/or other information referencing the vCard to which the information pertains
        /// When directory information is available from more than one source, the sending entity can pick what it considers to be the best source, or multiple SOURCE properties can be included
        /// </remarks>
        /// <example>
        /// SOURCE:ldap://ldap.example.com/cn=Babs%20Jensen,%20o=Babsco,%20c=US
        /// SOURCE:http://directory.example.com/addressbooks/jdoe/Jean%20Dupont.vcf
        /// </example>
        public Uri Source
        {
            get { return this.source; }
            set
            {
                this.source = value;
                OnPropertyChanged();
            }
        }
        private string SourceToString()
        {
            return SOURCE_PROPERTIES + VALUE_PARAMETER_SEPARATOR + this.Source.ToString() + NEW_LINE;
        }

        public interface LanguageSpecificVCarcObject
        {
            //public const string LANGUAGE

            private CultureInfo language;
            CultureInfo Language
            {
                get { return this.language; }
                set
                {
                    this.language = value;
                    OnPropertyChanged();
                }
            }

            public override string ToString()
            {
                return this.Language.TwoLetterISOLanguageName;
            }
        }

        public abstract class VCardItemValue : INotifyPropertyChanged
        {
            public const string VALUE_ARGUMENT_SEPARATOR = ";";

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public abstract class VCardItem : INotifyPropertyChanged
        {
            public const string VALUE_PARAMETER_SEPARATOR = ":";

            public abstract string NAME { get; }

            public abstract VCardItemValue Value { get; }

            public override string ToString()
            {
                return NAME + VALUE_PARAMETER_SEPARATOR + Value.ToString();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public const string XML_PROPERTIES = "XML";
        public Xml.XmlDocument XML;
        #endregion

        public Identity()
        {

        }
        public Identity(Stream vCardFile)
        {

        }
        public Identity(string vCardText)
        {

        }

        #region Identification properties
        //These types are used to capture information associated with the identification and naming of the entity associated with the vCard.

        public const string FORMATTED_NAME_COMPILED = "FN";
        /// <summary>
        /// To specify the formatted text corresponding to the name of the object the vCard represents
        /// </summary>
        /// <remarks>
        /// This property is based on the semantics of the X.520 Common Name attribute[CCITT.X520.1988]
        /// The property MUST be present in the vCard object
        /// </remarks>
        /// <example>FN:Mr. John Q. Public\, Esq.</example>
        public string FormattedName
        {
            get { return this.FormatName(); }
            set { this.ParseFormattedName(value); }
        }
        protected abstract void ParseFormattedName(string formattedName);
        protected abstract string FormatName();

        public const string NAME_COMPILED = "N";
        private string name;
        /// <summary>
        /// To specify the components of the name of the object the vCard represents
        /// </summary>
        /// <remarks>
        /// The structured property value corresponds, in sequence, to the Family Names(also known as surnames), Given Names, Additional Names, Honorific Prefixes, and Honorific Suffixes
        /// The text components are separated by the SEMICOLON character (U+003B)
        /// Individual text components can include multiple text values separated by the COMMA character(U+002C)
        /// This property is based on the semantics of the X.520 individual name attributes[CCITT.X520.1988]
        /// The property SHOULD be present in the vCard object when the name of the object the vCard represents follows the X.520 model
        /// </remarks>
        /// <example>
        /// N:Public;John;Quinlan;Mr.;Esq.
        /// N:Stevenson;John;Philip,Paul;Dr.;Jr.,M.D.,A.C.P.
        /// </example>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }

        public const string NICKNAME_COMPILED = "NICKNAME";
        /// <summary>
        /// To specify the text corresponding to the nickname of the object the vCard represents
        /// </summary>
        /// <remarks>
        /// The nickname is the descriptive name given instead of or in addition to the one belonging to the object the vCard represents
        /// It can also be used to specify a familiar form of a proper name specified by the FN or N properties
        /// </remarks>
        /// <example>
        /// NICKNAME:Robbie
        /// NICKNAME:Jim,Jimmie
        /// NICKNAME;TYPE=work:Boss
        /// </example>
        public ObservableCollection<string> Nicknames = new ObservableCollection<string>();

        public const string PHOTO_COMPILED = "PHOTO";
        private Image photo;
        /// <summary>
        /// To specify an image or photograph information that annotates some aspect of the object the vCard represents
        /// </summary>
        /// <example>
        /// PHOTO:http://www.example.com/pub/photos/jqpublic.gif
        /// PHOTO:data:image/jpeg;base64,MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvAQEEBQAwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05ldHNjYXBlIENvbW11bmljYXRpb25zIENvcnBvcmF0aW9uMRwwGgYDVQQLExNJbmZvcm1hdGlvbiBTeXN0<...remainder of base64-encoded data...>
        /// </example>
        public Image Photo
        {
            get { return this.photo; }
            set
            {
                this.photo = value;
                OnPropertyChanged();
            }
        }

        public const string BIRTHDAY_COMPILED = "BDAY";
        private DateTime? birthday;
        public DateTime? Birthday
        {
            get { return this.birthday; }
            set
            {
                this.birthday = value;
                OnPropertyChanged();

                if (this.Anniversary.HasValue == false)
                    this.Anniversary = value;
            }
        }

        public const string ANNIVERSARY_COMPILED = "ANNIVERSARY";
        private DateTime? anniversary;
        public DateTime? Anniversary
        {
            get { return this.anniversary; }
            set
            {
                this.anniversary = value;
                OnPropertyChanged();

                if (this.Birthday.HasValue == false)
                    this.Birthday = value;
            }
        }

        public const string GENDER_COMPILED = "GENDER";
        private Gender gender;
        public Gender Gender
        {
            get { return this.gender; }
            set
            {
                this.gender = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Delivery addressing properties
        //These types are concerned with information related to the delivery addressing or label for the vCard object.

        private Location address;
        public Location Address
        {
            get { return this.address; }
            set
            {
                this.address = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Communication propreties
        //These properties describe information about how to communicate with the object the vCard represents.

        public ObservableCollection<TelephoneUri> Telephones = new ObservableCollection<TelephoneUri>();

        public ObservableCollection<EmailURI> Emails = new ObservableCollection<EmailURI>();
        #endregion

        #region Organizational properties
        //These properties are concerned with information associated with characteristics of the organization or organizational units of the object that the vCard represents

        private string title;
        /// <summary>
        /// To specify the position or job of the object the vCard represents
        /// </summary>
        /// <remarks>This property is based on the X.520 Title attribute [CCITT.X520.1988]</remarks>
        /// <example>Research Scientist</example>
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }

        private string role;
        /// <summary>
        /// Function or part played in a particular situation by the object the vCard represents
        /// </summary>
        /// <remarks>
        /// This property is based on the X.520 Business Category explanatory attribute[CCITT.X520.1988]
        /// This property is included as an organizational type to avoid confusion with the semantics of the TITLE property and incorrect usage of that property when the semantics of this property is intended
        /// </remarks>
        /// <example>Project Leader</example>
        public string Role
        {
            get { return this.role; }
            set
            {
                this.role = value;
                OnPropertyChanged();
            }
        }

        private Image logo;
        /// <summary>
        /// Graphic image of a logo associated with the object the vCard represents
        /// </summary>
        /// <example>
        /// http://www.example.com/pub/logos/abccorp.jpg
        /// data:image/jpeg;base64,MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcAQEEBQAwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05ldHNjYXBlIENvbW11bmljYXRpb25zIENvcnBvcmF0aW9uMRwwGgYDVQQLExNJbmZvcm1hdGlvbiBTeXN0
        /// </example>
        public Image Logo
        {
            get { return this.logo; }
            set
            {
                this.logo = value;
                OnPropertyChanged();
            }
        }

        private string organisation;
        /// <summary>
        /// To specify the organizational name and units associated with the vCard
        /// </summary>
        /// <remarks>
        /// The property is based on the X.520 Organization Name and Organization Unit attributes[CCITT.X520.1988]
        /// The property value is a structured type consisting of the organization name, followed by zero or more levels of organizational unit names
        /// </remarks>
        /// <example>
        /// A property value consisting of an organizational name, organizational unit #1 name, and organizational unit #2 name
        /// ABC\, Inc.;North American Division;Marketing
        /// </example>
        public string Organisation
        {
            get { return this.organisation; }
            set
            {
                this.organisation = value;
                OnPropertyChanged();
            }
        }

        private Uri member;
        /// <summary>
        /// To include a member in the group this vCard represents
        /// </summary>
        /// <remarks>
        /// This property MUST NOT be present unless the value of the KIND property is "group"
        /// </remarks>
        /// <example>
        /// urn:uuid:03a0e51f-d1aa-4385-8a53-e29025acd8af
        /// urn:uuid:b8767877-b4a1-4c70-9acc-505d3819e519
        /// mailto:subscriber1@example.com
        /// xmpp:subscriber2 @example.com
        /// sip:subscriber3 @example.com
        /// tel:+1-418-555-5555
        /// </example>
        public Uri Member
        {
            get { return this.member; }
            set
            {
                this.member = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// To specify a relationship between another entity and the entity represented by this vCard
        /// </summary>
        /// <remarks>
        /// The TYPE parameter MAY be used to characterize the related entity
        /// It contains a comma-separated list of values that are registered with IANA as described in Section 10.2
        /// The registry is pre-populated with the values defined in [xfn]
        /// This document also specifies two additional values: 
        /// - agent:  an entity who may sometimes act on behalf of the entity associated with the vCard
        /// - emergency:  indicates an emergency contact
        /// </remarks>
        /// <example>
        /// TYPE=friend:urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6
        /// TYPE=contact:http://example.com/directory/jdoe.vcf
        /// TYPE=co-worker;VALUE=text:Please contact my assistant Jane Doe for any inquiries
        /// </example>
        public ObservableCollection<Relationship> Relationships = new ObservableCollection<Relationship>();
        #endregion

        #region Explanatory Properties
        //These properties are concerned with additional explanations, such as that related to informational notes or revisions specific to the vCard

        /// <summary>
        /// To specify application category information about the vCard, also known as "tags"
        /// </summary>
        /// <example>
        /// TRAVEL AGENT
        /// INTERNET,IETF,INDUSTRY,INFORMATION TECHNOLOGY
        /// </example>
        public ObservableCollection<string> Categories = new ObservableCollection<string>();

        private string note;
        /// <summary>
        /// To specify supplemental information or a comment that is associated with the vCard
        /// </summary>
        /// <remarks>
        /// The property is based on the X.520 Description attribute[CCITT.X520.1988]
        /// </remarks>
        /// <example>This fax number is operational 0800 to 1715 EST\, Mon-Fri.</example>
        public string Note
        {
            get { return this.note; }
            set
            {
                this.note = value;
                OnPropertyChanged();
            }
        }

        private string productID;
        /// <summary>
        /// To specify the identifier for the product that created the vCard object
        /// </summary>
        /// <remarks>
        /// Implementations SHOULD use a method such as that specified for Formal Public Identifiers in [ISO9070] or for Universal Resource Names in [RFC3406] to ensure that the text value is unique
        /// </remarks>
        /// <example>-//ONLINE DIRECTORY//NONSGML Version 1//EN</example>
        public string ProductID
        {
            get { return this.productID; }
            set
            {
                this.productID = value;
                OnPropertyChanged();
            }
        }

        private DateTime revision;
        /// <summary>
        /// To specify revision information about the current vCard
        /// </summary>
        /// <remarks>
        /// The value distinguishes the current revision of the information in this vCard for other renditions of the information
        /// </remarks>
        /// <example>19951031T222710Z</example>
        public DateTime Revision
        {
            get { return this.revision; }
            set
            {
                this.revision = value;
                OnPropertyChanged();
            }
        }

        private Stream sound;
        /// <summary>
        /// To specify a digital sound content information that annotates some aspect of the vCard
        /// This property is often used to specify the proper pronunciation of the name property value of the vCard
        /// </summary>
        /// <example>
        /// CID:JOHNQPUBLIC.part8.19960229T080000.xyzMail@example.com
        /// data:audio/basic;base64,MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhAQEEBQAwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05ldHNjYXBlIENvbW11bmljYXRpb25zIENvcnBvcmF0aW9uMRwwGgYDVQQLExNJbmZvcm1hdGlvbiBTeXN0
        /// </example>
        public Stream Sound
        {
            get { return this.sound; }
            set
            {
                this.sound = value;
                OnPropertyChanged();
            }
        }

        private Guid uid;
        /// <summary>
        /// To specify a value that represents a globally unique identifier corresponding to the entity associated with the vCard
        /// </summary>
        /// <remarks>
        /// This property is used to uniquely identify the object that the vCard represents
        /// The "uuid" URN namespace defined in [RFC4122] is particularly well suited to this task, but other URI schemes MAY be used
        /// Free-form text MAY also be used
        /// </remarks>
        /// <example>urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6</example>
        public Guid UID
        {
            get { return this.uid; }
            set
            {
                this.uid = value;
                OnPropertyChanged();
            }
        }

        //TODO: Implement CLIENTPIDMAP

        private URLUri url;
        /// <summary>
        /// To specify a uniform resource locator associated with the object to which the vCard refers
        /// Examples for individuals include personal web sites, blogs, and social networking site identifiers
        /// </summary>
        /// <example>http://example.org/restaurant.french/~chezchic.html</example>
        public URLUri URL
        {
            get { return this.url; }
            set
            {
                this.url = value;
                OnPropertyChanged();
            }
        }

        public const string VERSION_HEADER = "VERSION";
        /// <summary>
        /// To specify the version of the vCard specification used to format this vCard
        /// </summary>
        /// <remarks>
        /// This property MUST be present in the vCard object, and it must appear immediately after BEGIN:VCARD
        /// The value MUST be "4.0" if the vCard corresponds to this specification
        /// Note that earlier versions of vCard allowed this property to be placed anywhere in the vCard object, or even to be absent
        /// </remarks>
        public const float VERSION_VALUE = 4.0f;
        public static string VERSION_TEXT_VALUE = VERSION_VALUE.ToString("0.0");
        //TODO: Make this value costum in case of import
        #endregion

        #region Security properties
        //These properties are concerned with the security of communication pathways or access to the vCard

        private Stream key;
        /// <summary>
        /// To specify a public key or authentication certificate associated with the object that the vCard represents
        /// </summary>
        /// <example>
        /// http://www.example.com/keys/jdoe.cer
        /// MEDIATYPE=application/pgp-keys:ftp://example.com/keys/jdoe
        /// data:application/pgp-keys;base64,MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQAwdzELMAkGA1UEBhMCVVMxLDAqBgNVBAoTI05l
        /// </example>
        public Stream Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
                OnPropertyChanged();
            }
        }
        //TODO: Implement helpers
        #endregion

        #region Calendar properties
        //These properties are further specified in [RFC2739]

        //TODO: Introduce CalenderURI
        private Uri freeBusyCalendar;
        /// <summary>
        /// To specify the URI for the busy time associated with the object that the vCard represents
        /// </summary>
        /// <remarks>
        /// Where multiple FBURL properties are specified, the default FBURL property is indicated with the PREF parameter
        /// The FTP[RFC1738] or HTTP[RFC2616] type of URI points to an iCalendar [RFC5545] object associated with a snapshot of the next few weeks or months of busy time data
        /// If the iCalendar object is represented as a file or document, its file extension should be ".ifb"
        /// </remarks>
        /// <example>
        /// PREF=1:http://www.example.com/busy/janedoe
        /// MEDIATYPE=text/calendar:ftp://example.com/busy/project-a.ifb
        /// </example>
        public Uri FreeBusyCalendar
        {
            get { return this.freeBusyCalendar; }
            set
            {
                this.freeBusyCalendar = value;
                OnPropertyChanged();
            }
        }

        private EmailURI calendarEmail;
        /// <summary>
        /// To specify the calendar user address [RFC5545] to which a scheduling request[RFC5546] should be sent for the object represented by the vCard
        /// </summary>
        /// <remarks>
        /// Where multiple CALADRURI properties are specified, the default CALADRURI property is indicated with the PREF parameter
        /// </remarks>
        /// <example>
        /// PREF=1:mailto:janedoe@example.com
        /// http://example.com/calendar/jdoe
        /// </example>
        public EmailURI CalendarEmail
        {
            get { return this.calendarEmail; }
            set
            {
                this.calendarEmail = value;
                OnPropertyChanged();
            }
        }

        private Uri calendar;
        /// <summary>
        /// To specify the URI for a calendar associated with the object represented by the vCard
        /// </summary>
        /// <remarks>
        /// Where multiple CALURI properties are specified, the default CALURI property is indicated with the PREF parameter
        /// The property should contain a URI pointing to an iCalendar[RFC5545] object associated with a snapshot of the user's calendar store
        /// If the iCalendar object is represented as a file or document, its file extension should be ".ics"
        /// </remarks>
        /// <example>
        /// PREF=1:http://cal.example.com/calA
        /// MEDIATYPE=text/calendar:ftp://ftp.example.com/calA.ics
        /// </example>
        public Uri Calendar
        {
            get { return this.calendar; }
            set
            {
                this.calendar = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public void Clean()
        {
            //TODO: Clean for non defined in the standard properties
        }

        public void Merge(string other)
        {}
        public void Merge(Stream other)
        {

        }

        public string Serialized()
        {
            string vCard = BEGIN_HEADER + VALUE_PARAMETER_SEPARATOR + IDENTIFIER + NEW_LINE
                         + VERSION_HEADER + VALUE_PARAMETER_SEPARATOR + VERSION_TEXT_VALUE + NEW_LINE;



            vCard += END_FOOTER + VALUE_PARAMETER_SEPARATOR + IDENTIFIER + NEW_LINE;
            return vCard;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
