using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using AgapeEncryption;
using Account;

namespace KeyUser
{
    public class KeyUser
    {
        #region Constructors
        public KeyUser(string username, string password, string device)
        {
            this._username = username;
            this._password = password;
            this._device = device;


            _loggedSuccessful = Login(this._username, this._password, this._device);
        }

        public KeyUser(Guid keyGuid, Guid mobilePasscode)
        {
            this._keyGuid = keyGuid;
            this._mobilePasscode = mobilePasscode;

            _loggedSuccessful = Login(_keyGuid, _mobilePasscode);
        }
        #endregion

        #region Properties
        private int _id;
        private string _username;
        private string _password;
        private Guid _keyGuid;
        private Guid _mobilePasscode;
        private string _pgt;
        private string _device;
        private DateTime _dataAdded;
        private DateTime _lastDateModified;
        private bool _active;
        private bool _loggedSuccessful;

        public string KeyGuid
        {
            get { return _keyGuid.ToString(); }
        }

        public string MobilePasscode
        {
            get { return _mobilePasscode.ToString(); }
        }

        public string ProxyTicket
        {
            get { return _pgt; }
        }

        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public DateTime DateAdded
        {
            get { return _dataAdded; }
            set { _dataAdded = value; }
        }

        public DateTime LastDateModified
        {
            get { return _lastDateModified; }
            set { _lastDateModified = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public bool LogginSuccessful
        {
            get { return _loggedSuccessful; }
        }
        #endregion

        #region Methods
        ////First time login, for Key proxy
        public bool Login(string username, string password, string device) 
        {
            if (Authenticate(username, password))
            {
                //insert those into the DB, with Key credentials along with the device and date fields
                var checkDb = new KeyEntities().AP_KeyCredentials.Where(k => ((k.KeyGuid == _keyGuid) && (k.Device == device)));

                if (checkDb.Count() == 0)
                {
                    _mobilePasscode = Guid.NewGuid();
                    StoreKeyCredentials(username, password, device, _keyGuid, _mobilePasscode, DateTime.Now, DateTime.Now, true);
                }
                else
                {
                    if (checkDb.First().MobilePasscode != null)
                    {
                        _mobilePasscode = new Guid(checkDb.First().MobilePasscode.ToString());
                    }
                }

                return true;
            }
            else 
            {
                _keyGuid = Guid.Empty;
                _mobilePasscode = Guid.Empty;
                return false;
            }
        }
        
        ////Subsequent logins using "mobile" credentials
        public bool Login(Guid keyGuid, Guid mobilePasscode) 
        {
            if (GetKeyCredentials(keyGuid, mobilePasscode))
            {
                //mobile credentials found
                //result of authentication return true if authenticated... will need to get tickets from this
                return Authenticate(_username, _password);
            }
            else
            {
                //mobile credentials not found
                _keyGuid = Guid.Empty;
                _pgt = string.Empty;
                return false;
            }
        }

        private bool GetKeyCredentials(Guid keyGuid, Guid mobilePasscode) 
        { 
            //Using ssocode and mobilepasscode, get username and password

            var e = new KeyEntities().AP_KeyCredentials.Where(k => ((k.KeyGuid == keyGuid) && (k.MobilePasscode == mobilePasscode)));

            if (e.Count<AP_KeyCredentials>() > 0)
            {
                //If found, set the values
                if (!string.IsNullOrEmpty(e.First<AP_KeyCredentials>().Username))
                { _username = ADCEncrypt.Decrypt(e.First<AP_KeyCredentials>().Username); }
                else
                { _username = string.Empty; }

                if (!string.IsNullOrEmpty(e.First<AP_KeyCredentials>().Password))
                { _password = ADCEncrypt.Decrypt(e.First<AP_KeyCredentials>().Password); }
                else
                { _username = string.Empty; }

                _device = e.First<AP_KeyCredentials>().Device;

                return true;
            }
            else
            {
                //if not found, return false
                _username = string.Empty;
                _password = string.Empty;
                return true;
            }
        }

        private bool Authenticate(string username, string password)
        {
            KeyAuthentication keyAuth = new KeyAuthentication(username, password);

            if (string.IsNullOrEmpty(keyAuth.KeyGuid))
            {
                _pgt = string.Empty;
                return false;
            }
            else
            {
                _keyGuid = new Guid(keyAuth.KeyGuid);
                _pgt = keyAuth.TicketGrantingTicket;

                return true;
            }
        }

        private void StoreKeyCredentials(string username, string password, string device, Guid keyGuid, Guid mobilePasscode, DateTime dateAdded, DateTime lastModified, bool active)
        {
            string EncryptedUserName = ADCEncrypt.Encrypt(username);
            string EncryptedPassword = ADCEncrypt.Encrypt(password);

            AP_KeyCredentials keyCred = new AP_KeyCredentials();

            keyCred.Username = EncryptedUserName;
            keyCred.Password = EncryptedPassword;
            keyCred.KeyGuid = keyGuid;
            keyCred.MobilePasscode = mobilePasscode;
            keyCred.Device = device;
            keyCred.LastModified = lastModified;
            keyCred.DateAdded = dateAdded;
            keyCred.IsActive = active;

            StoreKeyCredentials(keyCred);
        }

        private static void StoreKeyCredentials(AP_KeyCredentials keyCred)
        {
            KeyEntities acEntity = new KeyEntities();

            try
            {
                acEntity.AddToAP_KeyCredentials(keyCred);
                acEntity.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        //public static bool DeviceStatus(bool activated) { return true; }
        //public static void DeactivateDevice() { }
        #endregion
    }
}