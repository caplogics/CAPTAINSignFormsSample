using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Interfaces;
using Captain.Common.Utilities;
using Captain.Common.Controllers;

namespace Captain.Common.Model.Data
{
    public class CaptainModel : ICaptainModel
    {
        private AuthenticateUser _authenticateUser = null;
        private UserProfileAccess _userProfileAccess = null;
        private NavigationData _navigateData = null;
        private LookupDataAccess _lookupDataAccess = null;
        private ZipCodeAndAgency _zipcodeandagency = null;
        private HierarchyAndPrograms _hierarchyAndPrograms = null;
        private MasterPovertyData _masterPovertyData = null;
        private Agytab _agytab = null;
        private CaseMstData _caseMSTData = null;
        private FieldControls _fieldControls = null;
        private TmsApcnData _tmsApcndata = null;
        private TMS00110Data _tms00110Data = null;
        private LiheAllData _liheAllData = null;
        private ADMNB002Data _admnb002Data = null;
        private SPAdminData _spadminData = null;
        private CaseSumData _caseSumData = null;
        private AdhocData _adhocdata = null;
        private MatrixScalesData _matrixscalesData = null;
        private STAFFData _staffData = null;
        private InkindData _inkindData = null;
        private ChldMstData _chldmstData = null;
        private ChldTrckData _chldTrckData = null;
        private EnrollData _enrolldata = null;
        private MainMenuData _mainmenudata = null;
        private ChldAttnData _chldAttnData = null;
        private PIRData _pirData = null;
        private ArsData _arsData = null;
        private TmsAllData _tmsAllData = null;
        private EMSBDCData _emsbdcData = null;
        private HlsTrckData _hlsTrckData = null;
        private CrossWalkData _CrossWalkData = null;
        private SmartChoiceData _SmartChoiceData = null;

        private HUDCNTLData _hUDCNTLData = null;

        private SaldefData _saldefData = null;

        public CaptainModel()
        {

        }

        public string UserId
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.UserID, Consts.Common.DefaultUserID];
            }
        }

        public string UserName
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.UserName, string.Empty];
            }
        }

        public string Locale
        {
            get
            {
                return Captain<string>.Session[Consts.SessionVariables.Language, Consts.Common.DefaultLanguage];
            }
        }

        public AuthenticateUser AuthenticateUser
        {
            get
            {
                if (_authenticateUser == null) { _authenticateUser = new AuthenticateUser(this); }
                return _authenticateUser;
            }
        }

        public UserProfileAccess UserProfileAccess
        {
            get
            {
                if (_userProfileAccess == null) { _userProfileAccess = new UserProfileAccess(this); }
                return _userProfileAccess;
            }
        }

        public NavigationData NavigationData
        {
            get
            {
                if (_navigateData == null) { _navigateData = new NavigationData(this); }
                return _navigateData;
            }
        }

        public LookupDataAccess lookupDataAccess
        {
            get
            {
                if (_lookupDataAccess == null) { _lookupDataAccess = new LookupDataAccess(this); }
                return _lookupDataAccess;
            }
        }

        public ZipCodeAndAgency ZipCodeAndAgency
        {
            get
            {
                if (_zipcodeandagency == null) { _zipcodeandagency = new ZipCodeAndAgency(); }
                return _zipcodeandagency;
            }
        }

        public HierarchyAndPrograms HierarchyAndPrograms
        {
            get
            {
                if (_hierarchyAndPrograms == null) { _hierarchyAndPrograms = new HierarchyAndPrograms(this); }
                return _hierarchyAndPrograms;
            }
        }

        public MasterPovertyData masterPovertyData
        {
            get
            {
                if (_masterPovertyData == null) { _masterPovertyData = new MasterPovertyData(); }
                return _masterPovertyData;
            }
        }

        public Agytab Agytab
        {
            get
            {
                if (_agytab == null) { _agytab = new Agytab(); }
                return _agytab;
            }
        }

        public CaseMstData CaseMstData
        {
            get
            {
                if (_caseMSTData == null) { _caseMSTData = new CaseMstData(this); }
                return _caseMSTData;
            }
        }

        public FieldControls FieldControls
        {
            get
            {
                if (_fieldControls == null) { _fieldControls = new FieldControls(); }
                return _fieldControls;
            }
        }

        public TmsApcnData TmsApcndata
        {
            get
            {
                if (_tmsApcndata == null) { _tmsApcndata = new TmsApcnData(this); }
                return _tmsApcndata;
            }
        }

        public TMS00110Data TMS00110Data
        {
            get
            {
                if (_tms00110Data == null) { _tms00110Data = new TMS00110Data(); }
                return _tms00110Data;
            }
        }

        public LiheAllData LiheAllData
        {
            get
            {
                if (_liheAllData == null) { _liheAllData = new LiheAllData(this); }
                return _liheAllData;
            }
        }

        public ADMNB002Data ADMNB002Data
        {
            get
            {
                if (_admnb002Data == null) { _admnb002Data = new ADMNB002Data(); }
                return _admnb002Data;
            }
        }

        public SPAdminData SPAdminData
        {
            get
            {
                if (_spadminData == null) { _spadminData = new SPAdminData(); }
                return _spadminData;
            }
        }

        public CaseSumData CaseSumData
        {
            get
            {
                if (_caseSumData == null) { _caseSumData = new CaseSumData(this); }
                return _caseSumData;
            }
        }

        public AdhocData AdhocData
        {
            get
            {
                if (_adhocdata == null) { _adhocdata = new AdhocData(); }
                return _adhocdata;
            }
        }


        public MatrixScalesData MatrixScalesData
        {
            get
            {
                if (_matrixscalesData == null) { _matrixscalesData = new MatrixScalesData(); }
                return _matrixscalesData;
            }
        }


        public STAFFData STAFFData
        {
            get
            {
                if (_staffData == null) { _staffData = new STAFFData(); }
                return _staffData;
            }
        }

        public InkindData INKINDData
        {
            get
            {
                if (_inkindData == null) { _inkindData = new InkindData(); }
                return _inkindData;
            }
        }

        public ChldMstData ChldMstData
        {
            get
            {
                if (_chldmstData == null) { _chldmstData = new ChldMstData(this); }
                return _chldmstData;
            }
        }
        //Uncommented by vikash for Matrix Scales Measures Report 11/29/22
        public ChldTrckData ChldTrckData
        {
            get
            {
                if (_chldTrckData == null) { _chldTrckData = new ChldTrckData(this); }
                return _chldTrckData;
            }
        }
        public EnrollData EnrollData
        {
            get
            {
                if (_enrolldata == null) { _enrolldata = new EnrollData(); }
                return _enrolldata;
            }
        }


        public MainMenuData MainMenuData
        {
            get
            {
                if (_mainmenudata == null) { _mainmenudata = new MainMenuData(); }
                return _mainmenudata;
            }
        }

        public ChldAttnData ChldAttnData
        {
            get
            {
                if (_chldAttnData == null) { _chldAttnData = new ChldAttnData(this); }
                return _chldAttnData;
            }
        }

        public PIRData PIRData
        {
            get
            {
                if (_pirData == null) { _pirData = new PIRData(); }
                return _pirData;
            }
        }

        public ArsData ArsData
        {
            get
            {
                if (_arsData == null) { _arsData = new ArsData(this); }
                return _arsData;
            }
        }
        public TmsAllData TmsAllData
        {
            get
            {
                if (_tmsAllData == null) { _tmsAllData = new TmsAllData(this); }
                return _tmsAllData;
            }
        }

        public EMSBDCData EMSBDCData
        {
            get
            {
                if (_emsbdcData == null) { _emsbdcData = new EMSBDCData(this); }
                return _emsbdcData;
            }
        }
        //Uncommented by vikash for Matrix Scales Measures Report 11/29/22
        public HlsTrckData HlsTrckData
        {
            get
            {
                if (_hlsTrckData == null) { _hlsTrckData = new HlsTrckData(this); }
                return _hlsTrckData;
            }
        }

        public SaldefData SALDEFData
        {
            get
            {
                if (_saldefData == null) { _saldefData = new SaldefData(this); }
                return _saldefData;
            }
        }

        public CrossWalkData CrosswalkData
        {
            get
            {
                if (_CrossWalkData == null) { _CrossWalkData = new CrossWalkData(); }
                return _CrossWalkData;
            }
        }
        public SmartChoiceData SmartChoiceData
        {
            get
            {
                if (_SmartChoiceData == null) { _SmartChoiceData = new SmartChoiceData(); }
                return _SmartChoiceData;
            }
        }


        #region Vikash added on 08/07/2024 for HUD Control

        public HUDCNTLData HUDCNTLData
        {
            get
            {
                if (_hUDCNTLData == null)
                {
                    _hUDCNTLData = new HUDCNTLData(this);
                }
                return _hUDCNTLData;
            }
        }

        #endregion
    }
}
