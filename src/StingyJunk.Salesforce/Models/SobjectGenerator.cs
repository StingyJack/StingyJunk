

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable InconsistentNaming
namespace StingyJunk.Salesforce.Models 
{
	public partial class Order
	{
		public string Type {get;set;}
		public string BillingStreet {get;set;}
		public string BillingCity {get;set;}
		public string BillingState {get;set;}
		public string BillingPostalCode {get;set;}
		public string BillingCountry {get;set;}
		public string BillingLatitude {get;set;}
		public string BillingLongitude {get;set;}
		public string BillingGeocodeAccuracy {get;set;}
		public string BillingAddress {get;set;}
		public string ShippingStreet {get;set;}
		public string ShippingCity {get;set;}
		public string ShippingState {get;set;}
		public string ShippingPostalCode {get;set;}
		public string ShippingCountry {get;set;}
		public string ShippingLatitude {get;set;}
		public string ShippingLongitude {get;set;}
		public string ShippingGeocodeAccuracy {get;set;}
		public string ShippingAddress {get;set;}
		public string Name {get;set;}
		public string PoDate {get;set;}
		public string PoNumber {get;set;}
		public string OrderReferenceNumber {get;set;}
		public string BillToContactId {get;set;}
		public string ShipToContactId {get;set;}
		public string ActivatedDate {get;set;}
		public string ActivatedById {get;set;}
		public string StatusCode {get;set;}
		public string OrderNumber {get;set;}
		public string TotalAmount {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string IsDeleted {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string ContractId {get;set;}
		public string AccountId {get;set;}
		public string Pricebook2Id {get;set;}
		public string OriginalOrderId {get;set;}
		public string OpportunityId {get;set;}
		public string EffectiveDate {get;set;}
		public string EndDate {get;set;}
		public string IsReductionOrder {get;set;}
		public string Status {get;set;}
		public string Description {get;set;}
		public string CustomerAuthorizedById {get;set;}
		public string CustomerAuthorizedDate {get;set;}
		public string CompanyAuthorizedById {get;set;}
		public string CompanyAuthorizedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Type, BillingStreet, BillingCity, BillingState, BillingPostalCode, BillingCountry, BillingLatitude, BillingLongitude, BillingGeocodeAccuracy, BillingAddress, ShippingStreet, ShippingCity, ShippingState, ShippingPostalCode, ShippingCountry, ShippingLatitude, ShippingLongitude, ShippingGeocodeAccuracy, ShippingAddress, Name, PoDate, PoNumber, OrderReferenceNumber, BillToContactId, ShipToContactId, ActivatedDate, ActivatedById, StatusCode, OrderNumber, TotalAmount, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, IsDeleted, SystemModstamp, LastViewedDate, LastReferencedDate, Id, OwnerId, ContractId, AccountId, Pricebook2Id, OriginalOrderId, OpportunityId, EffectiveDate, EndDate, IsReductionOrder, Status, Description, CustomerAuthorizedById, CustomerAuthorizedDate, CompanyAuthorizedById, CompanyAuthorizedDate FROM Order";
		}
	}

	public partial class Lead
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string MasterRecordId {get;set;}
		public string LastName {get;set;}
		public string FirstName {get;set;}
		public string Salutation {get;set;}
		public string Name {get;set;}
		public string Title {get;set;}
		public string Company {get;set;}
		public string Street {get;set;}
		public string City {get;set;}
		public string State {get;set;}
		public string PostalCode {get;set;}
		public string Country {get;set;}
		public string Latitude {get;set;}
		public string Longitude {get;set;}
		public string GeocodeAccuracy {get;set;}
		public string Address {get;set;}
		public string Phone {get;set;}
		public string MobilePhone {get;set;}
		public string Fax {get;set;}
		public string Email {get;set;}
		public string Website {get;set;}
		public string PhotoUrl {get;set;}
		public string Description {get;set;}
		public string LeadSource {get;set;}
		public string Status {get;set;}
		public string Industry {get;set;}
		public string Rating {get;set;}
		public string AnnualRevenue {get;set;}
		public string NumberOfEmployees {get;set;}
		public string OwnerId {get;set;}
		public string HasOptedOutOfEmail {get;set;}
		public string IsConverted {get;set;}
		public string ConvertedDate {get;set;}
		public string ConvertedAccountId {get;set;}
		public string ConvertedContactId {get;set;}
		public string ConvertedOpportunityId {get;set;}
		public string IsUnreadByOwner {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string CreatedDate {get;set;}
		public string LastActivityDate {get;set;}
		public string DoNotCall {get;set;}
		public string HasOptedOutOfFax {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string LastTransferDate {get;set;}
		public string Jigsaw {get;set;}
		public string JigsawContactId {get;set;}
		public string CleanStatus {get;set;}
		public string CompanyDunsNumber {get;set;}
		public string DandbCompanyId {get;set;}
		public string EmailBouncedReason {get;set;}
		public string EmailBouncedDate {get;set;}
		public string SICCode__c {get;set;}
		public string ProductInterest__c {get;set;}
		public string Primary__c {get;set;}
		public string CurrentGenerators__c {get;set;}
		public string NumberofLocations__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, MasterRecordId, LastName, FirstName, Salutation, Name, Title, Company, Street, City, State, PostalCode, Country, Latitude, Longitude, GeocodeAccuracy, Address, Phone, MobilePhone, Fax, Email, Website, PhotoUrl, Description, LeadSource, Status, Industry, Rating, AnnualRevenue, NumberOfEmployees, OwnerId, HasOptedOutOfEmail, IsConverted, ConvertedDate, ConvertedAccountId, ConvertedContactId, ConvertedOpportunityId, IsUnreadByOwner, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, CreatedDate, LastActivityDate, DoNotCall, HasOptedOutOfFax, LastViewedDate, LastReferencedDate, LastTransferDate, Jigsaw, JigsawContactId, CleanStatus, CompanyDunsNumber, DandbCompanyId, EmailBouncedReason, EmailBouncedDate, SICCode__c, ProductInterest__c, Primary__c, CurrentGenerators__c, NumberofLocations__c FROM Lead";
		}
	}

	public partial class Solution
	{
		public string SystemModstamp {get;set;}
		public string TimesUsed {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string IsHtml {get;set;}
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string SolutionNumber {get;set;}
		public string SolutionName {get;set;}
		public string IsPublished {get;set;}
		public string IsPublishedInPublicKb {get;set;}
		public string Status {get;set;}
		public string IsReviewed {get;set;}
		public string SolutionNote {get;set;}
		public string OwnerId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT SystemModstamp, TimesUsed, LastViewedDate, LastReferencedDate, IsHtml, Id, IsDeleted, SolutionNumber, SolutionName, IsPublished, IsPublishedInPublicKb, Status, IsReviewed, SolutionNote, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById FROM Solution";
		}
	}

	public partial class User
	{
		public string ReceivesInfoEmails {get;set;}
		public string ReceivesAdminInfoEmails {get;set;}
		public string EmailEncodingKey {get;set;}
		public string ProfileId {get;set;}
		public string UserType {get;set;}
		public string LanguageLocaleKey {get;set;}
		public string EmployeeNumber {get;set;}
		public string DelegatedApproverId {get;set;}
		public string ManagerId {get;set;}
		public string LastLoginDate {get;set;}
		public string LastPasswordChangeDate {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string OfflineTrialExpirationDate {get;set;}
		public string OfflinePdaTrialExpirationDate {get;set;}
		public string UserPermissionsMarketingUser {get;set;}
		public string UserPermissionsOfflineUser {get;set;}
		public string UserPermissionsCallCenterAutoLogin {get;set;}
		public string UserPermissionsMobileUser {get;set;}
		public string UserPermissionsSFContentUser {get;set;}
		public string UserPermissionsKnowledgeUser {get;set;}
		public string UserPermissionsInteractionUser {get;set;}
		public string UserPermissionsSupportUser {get;set;}
		public string UserPermissionsJigsawProspectingUser {get;set;}
		public string UserPermissionsSiteforceContributorUser {get;set;}
		public string UserPermissionsSiteforcePublisherUser {get;set;}
		public string UserPermissionsWorkDotComUserFeature {get;set;}
		public string ForecastEnabled {get;set;}
		public string UserPreferencesActivityRemindersPopup {get;set;}
		public string UserPreferencesEventRemindersCheckboxDefault {get;set;}
		public string UserPreferencesTaskRemindersCheckboxDefault {get;set;}
		public string UserPreferencesReminderSoundOff {get;set;}
		public string UserPreferencesDisableAllFeedsEmail {get;set;}
		public string UserPreferencesDisableFollowersEmail {get;set;}
		public string UserPreferencesDisableProfilePostEmail {get;set;}
		public string UserPreferencesDisableChangeCommentEmail {get;set;}
		public string UserPreferencesDisableLaterCommentEmail {get;set;}
		public string UserPreferencesDisProfPostCommentEmail {get;set;}
		public string UserPreferencesContentNoEmail {get;set;}
		public string UserPreferencesContentEmailAsAndWhen {get;set;}
		public string UserPreferencesApexPagesDeveloperMode {get;set;}
		public string UserPreferencesHideCSNGetChatterMobileTask {get;set;}
		public string UserPreferencesDisableMentionsPostEmail {get;set;}
		public string UserPreferencesDisMentionsCommentEmail {get;set;}
		public string UserPreferencesHideCSNDesktopTask {get;set;}
		public string UserPreferencesHideChatterOnboardingSplash {get;set;}
		public string UserPreferencesHideSecondChatterOnboardingSplash {get;set;}
		public string UserPreferencesDisCommentAfterLikeEmail {get;set;}
		public string UserPreferencesDisableLikeEmail {get;set;}
		public string UserPreferencesSortFeedByComment {get;set;}
		public string UserPreferencesDisableMessageEmail {get;set;}
		public string UserPreferencesJigsawListUser {get;set;}
		public string UserPreferencesDisableBookmarkEmail {get;set;}
		public string UserPreferencesDisableSharePostEmail {get;set;}
		public string UserPreferencesEnableAutoSubForFeeds {get;set;}
		public string UserPreferencesDisableFileShareNotificationsForApi {get;set;}
		public string UserPreferencesShowTitleToExternalUsers {get;set;}
		public string UserPreferencesShowManagerToExternalUsers {get;set;}
		public string UserPreferencesShowEmailToExternalUsers {get;set;}
		public string UserPreferencesShowWorkPhoneToExternalUsers {get;set;}
		public string UserPreferencesShowMobilePhoneToExternalUsers {get;set;}
		public string UserPreferencesShowFaxToExternalUsers {get;set;}
		public string UserPreferencesShowStreetAddressToExternalUsers {get;set;}
		public string UserPreferencesShowCityToExternalUsers {get;set;}
		public string UserPreferencesShowStateToExternalUsers {get;set;}
		public string UserPreferencesShowPostalCodeToExternalUsers {get;set;}
		public string UserPreferencesShowCountryToExternalUsers {get;set;}
		public string UserPreferencesShowProfilePicToGuestUsers {get;set;}
		public string UserPreferencesShowTitleToGuestUsers {get;set;}
		public string UserPreferencesShowCityToGuestUsers {get;set;}
		public string UserPreferencesShowStateToGuestUsers {get;set;}
		public string UserPreferencesShowPostalCodeToGuestUsers {get;set;}
		public string UserPreferencesShowCountryToGuestUsers {get;set;}
		public string UserPreferencesDisableFeedbackEmail {get;set;}
		public string UserPreferencesDisableWorkEmail {get;set;}
		public string UserPreferencesHideS1BrowserUI {get;set;}
		public string UserPreferencesDisableEndorsementEmail {get;set;}
		public string UserPreferencesPathAssistantCollapsed {get;set;}
		public string UserPreferencesCacheDiagnostics {get;set;}
		public string UserPreferencesShowEmailToGuestUsers {get;set;}
		public string UserPreferencesShowManagerToGuestUsers {get;set;}
		public string UserPreferencesShowWorkPhoneToGuestUsers {get;set;}
		public string UserPreferencesShowMobilePhoneToGuestUsers {get;set;}
		public string UserPreferencesShowFaxToGuestUsers {get;set;}
		public string UserPreferencesShowStreetAddressToGuestUsers {get;set;}
		public string UserPreferencesLightningExperiencePreferred {get;set;}
		public string UserPreferencesPreviewLightning {get;set;}
		public string UserPreferencesHideEndUserOnboardingAssistantModal {get;set;}
		public string UserPreferencesHideLightningMigrationModal {get;set;}
		public string UserPreferencesHideSfxWelcomeMat {get;set;}
		public string UserPreferencesHideBiggerPhotoCallout {get;set;}
		public string ContactId {get;set;}
		public string AccountId {get;set;}
		public string CallCenterId {get;set;}
		public string Extension {get;set;}
		public string FederationIdentifier {get;set;}
		public string AboutMe {get;set;}
		public string FullPhotoUrl {get;set;}
		public string SmallPhotoUrl {get;set;}
		public string MediumPhotoUrl {get;set;}
		public string DigestFrequency {get;set;}
		public string DefaultGroupNotificationFrequency {get;set;}
		public string JigsawImportLimitOverride {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string BannerPhotoUrl {get;set;}
		public string IsProfilePhotoActive {get;set;}
		public string Id {get;set;}
		public string Username {get;set;}
		public string LastName {get;set;}
		public string FirstName {get;set;}
		public string Name {get;set;}
		public string CompanyName {get;set;}
		public string Division {get;set;}
		public string Department {get;set;}
		public string Title {get;set;}
		public string Street {get;set;}
		public string City {get;set;}
		public string State {get;set;}
		public string PostalCode {get;set;}
		public string Country {get;set;}
		public string Latitude {get;set;}
		public string Longitude {get;set;}
		public string GeocodeAccuracy {get;set;}
		public string Address {get;set;}
		public string Email {get;set;}
		public string EmailPreferencesAutoBcc {get;set;}
		public string EmailPreferencesAutoBccStayInTouch {get;set;}
		public string EmailPreferencesStayInTouchReminder {get;set;}
		public string SenderEmail {get;set;}
		public string SenderName {get;set;}
		public string Signature {get;set;}
		public string StayInTouchSubject {get;set;}
		public string StayInTouchSignature {get;set;}
		public string StayInTouchNote {get;set;}
		public string Phone {get;set;}
		public string Fax {get;set;}
		public string MobilePhone {get;set;}
		public string Alias {get;set;}
		public string CommunityNickname {get;set;}
		public string BadgeText {get;set;}
		public string IsActive {get;set;}
		public string TimeZoneSidKey {get;set;}
		public string UserRoleId {get;set;}
		public string LocaleSidKey {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT ReceivesInfoEmails, ReceivesAdminInfoEmails, EmailEncodingKey, ProfileId, UserType, LanguageLocaleKey, EmployeeNumber, DelegatedApproverId, ManagerId, LastLoginDate, LastPasswordChangeDate, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, OfflineTrialExpirationDate, OfflinePdaTrialExpirationDate, UserPermissionsMarketingUser, UserPermissionsOfflineUser, UserPermissionsCallCenterAutoLogin, UserPermissionsMobileUser, UserPermissionsSFContentUser, UserPermissionsKnowledgeUser, UserPermissionsInteractionUser, UserPermissionsSupportUser, UserPermissionsJigsawProspectingUser, UserPermissionsSiteforceContributorUser, UserPermissionsSiteforcePublisherUser, UserPermissionsWorkDotComUserFeature, ForecastEnabled, UserPreferencesActivityRemindersPopup, UserPreferencesEventRemindersCheckboxDefault, UserPreferencesTaskRemindersCheckboxDefault, UserPreferencesReminderSoundOff, UserPreferencesDisableAllFeedsEmail, UserPreferencesDisableFollowersEmail, UserPreferencesDisableProfilePostEmail, UserPreferencesDisableChangeCommentEmail, UserPreferencesDisableLaterCommentEmail, UserPreferencesDisProfPostCommentEmail, UserPreferencesContentNoEmail, UserPreferencesContentEmailAsAndWhen, UserPreferencesApexPagesDeveloperMode, UserPreferencesHideCSNGetChatterMobileTask, UserPreferencesDisableMentionsPostEmail, UserPreferencesDisMentionsCommentEmail, UserPreferencesHideCSNDesktopTask, UserPreferencesHideChatterOnboardingSplash, UserPreferencesHideSecondChatterOnboardingSplash, UserPreferencesDisCommentAfterLikeEmail, UserPreferencesDisableLikeEmail, UserPreferencesSortFeedByComment, UserPreferencesDisableMessageEmail, UserPreferencesJigsawListUser, UserPreferencesDisableBookmarkEmail, UserPreferencesDisableSharePostEmail, UserPreferencesEnableAutoSubForFeeds, UserPreferencesDisableFileShareNotificationsForApi, UserPreferencesShowTitleToExternalUsers, UserPreferencesShowManagerToExternalUsers, UserPreferencesShowEmailToExternalUsers, UserPreferencesShowWorkPhoneToExternalUsers, UserPreferencesShowMobilePhoneToExternalUsers, UserPreferencesShowFaxToExternalUsers, UserPreferencesShowStreetAddressToExternalUsers, UserPreferencesShowCityToExternalUsers, UserPreferencesShowStateToExternalUsers, UserPreferencesShowPostalCodeToExternalUsers, UserPreferencesShowCountryToExternalUsers, UserPreferencesShowProfilePicToGuestUsers, UserPreferencesShowTitleToGuestUsers, UserPreferencesShowCityToGuestUsers, UserPreferencesShowStateToGuestUsers, UserPreferencesShowPostalCodeToGuestUsers, UserPreferencesShowCountryToGuestUsers, UserPreferencesDisableFeedbackEmail, UserPreferencesDisableWorkEmail, UserPreferencesHideS1BrowserUI, UserPreferencesDisableEndorsementEmail, UserPreferencesPathAssistantCollapsed, UserPreferencesCacheDiagnostics, UserPreferencesShowEmailToGuestUsers, UserPreferencesShowManagerToGuestUsers, UserPreferencesShowWorkPhoneToGuestUsers, UserPreferencesShowMobilePhoneToGuestUsers, UserPreferencesShowFaxToGuestUsers, UserPreferencesShowStreetAddressToGuestUsers, UserPreferencesLightningExperiencePreferred, UserPreferencesPreviewLightning, UserPreferencesHideEndUserOnboardingAssistantModal, UserPreferencesHideLightningMigrationModal, UserPreferencesHideSfxWelcomeMat, UserPreferencesHideBiggerPhotoCallout, ContactId, AccountId, CallCenterId, Extension, FederationIdentifier, AboutMe, FullPhotoUrl, SmallPhotoUrl, MediumPhotoUrl, DigestFrequency, DefaultGroupNotificationFrequency, JigsawImportLimitOverride, LastViewedDate, LastReferencedDate, BannerPhotoUrl, IsProfilePhotoActive, Id, Username, LastName, FirstName, Name, CompanyName, Division, Department, Title, Street, City, State, PostalCode, Country, Latitude, Longitude, GeocodeAccuracy, Address, Email, EmailPreferencesAutoBcc, EmailPreferencesAutoBccStayInTouch, EmailPreferencesStayInTouchReminder, SenderEmail, SenderName, Signature, StayInTouchSubject, StayInTouchSignature, StayInTouchNote, Phone, Fax, MobilePhone, Alias, CommunityNickname, BadgeText, IsActive, TimeZoneSidKey, UserRoleId, LocaleSidKey FROM User";
		}
	}

	public partial class Account
	{
		public string Industry {get;set;}
		public string AnnualRevenue {get;set;}
		public string NumberOfEmployees {get;set;}
		public string Ownership {get;set;}
		public string TickerSymbol {get;set;}
		public string Description {get;set;}
		public string Rating {get;set;}
		public string Site {get;set;}
		public string OwnerId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string Jigsaw {get;set;}
		public string JigsawCompanyId {get;set;}
		public string CleanStatus {get;set;}
		public string AccountSource {get;set;}
		public string DunsNumber {get;set;}
		public string Tradestyle {get;set;}
		public string NaicsCode {get;set;}
		public string NaicsDesc {get;set;}
		public string YearStarted {get;set;}
		public string SicDesc {get;set;}
		public string DandbCompanyId {get;set;}
		public string CustomerPriority__c {get;set;}
		public string SLA__c {get;set;}
		public string Active__c {get;set;}
		public string NumberofLocations__c {get;set;}
		public string UpsellOpportunity__c {get;set;}
		public string SLASerialNumber__c {get;set;}
		public string SLAExpirationDate__c {get;set;}
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string MasterRecordId {get;set;}
		public string Name {get;set;}
		public string Type {get;set;}
		public string ParentId {get;set;}
		public string BillingStreet {get;set;}
		public string BillingCity {get;set;}
		public string BillingState {get;set;}
		public string BillingPostalCode {get;set;}
		public string BillingCountry {get;set;}
		public string BillingLatitude {get;set;}
		public string BillingLongitude {get;set;}
		public string BillingGeocodeAccuracy {get;set;}
		public string BillingAddress {get;set;}
		public string ShippingStreet {get;set;}
		public string ShippingCity {get;set;}
		public string ShippingState {get;set;}
		public string ShippingPostalCode {get;set;}
		public string ShippingCountry {get;set;}
		public string ShippingLatitude {get;set;}
		public string ShippingLongitude {get;set;}
		public string ShippingGeocodeAccuracy {get;set;}
		public string ShippingAddress {get;set;}
		public string Phone {get;set;}
		public string Fax {get;set;}
		public string AccountNumber {get;set;}
		public string Website {get;set;}
		public string PhotoUrl {get;set;}
		public string Sic {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Industry, AnnualRevenue, NumberOfEmployees, Ownership, TickerSymbol, Description, Rating, Site, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, LastViewedDate, LastReferencedDate, Jigsaw, JigsawCompanyId, CleanStatus, AccountSource, DunsNumber, Tradestyle, NaicsCode, NaicsDesc, YearStarted, SicDesc, DandbCompanyId, CustomerPriority__c, SLA__c, Active__c, NumberofLocations__c, UpsellOpportunity__c, SLASerialNumber__c, SLAExpirationDate__c, Id, IsDeleted, MasterRecordId, Name, Type, ParentId, BillingStreet, BillingCity, BillingState, BillingPostalCode, BillingCountry, BillingLatitude, BillingLongitude, BillingGeocodeAccuracy, BillingAddress, ShippingStreet, ShippingCity, ShippingState, ShippingPostalCode, ShippingCountry, ShippingLatitude, ShippingLongitude, ShippingGeocodeAccuracy, ShippingAddress, Phone, Fax, AccountNumber, Website, PhotoUrl, Sic FROM Account";
		}
	}

	public partial class Product2
	{
		public string Id {get;set;}
		public string Name {get;set;}
		public string ProductCode {get;set;}
		public string Description {get;set;}
		public string IsActive {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string Family {get;set;}
		public string IsDeleted {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, Name, ProductCode, Description, IsActive, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, Family, IsDeleted, LastViewedDate, LastReferencedDate FROM Product2";
		}
	}

	public partial class Goal
	{
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string Description {get;set;}
		public string ImageUrl {get;set;}
		public string DueDate {get;set;}
		public string CompletionDate {get;set;}
		public string StartDate {get;set;}
		public string IsKeyCompanyGoal {get;set;}
		public string Status {get;set;}
		public string Progress {get;set;}
		public string OrigGoalId__c {get;set;}
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, Description, ImageUrl, DueDate, CompletionDate, StartDate, IsKeyCompanyGoal, Status, Progress, OrigGoalId__c, Id, OwnerId, IsDeleted, Name, CreatedDate FROM Goal";
		}
	}

	public partial class Asset
	{
		public string Id {get;set;}
		public string ContactId {get;set;}
		public string AccountId {get;set;}
		public string ParentId {get;set;}
		public string RootAssetId {get;set;}
		public string Product2Id {get;set;}
		public string IsCompetitorProduct {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string SerialNumber {get;set;}
		public string InstallDate {get;set;}
		public string PurchaseDate {get;set;}
		public string UsageEndDate {get;set;}
		public string Status {get;set;}
		public string Price {get;set;}
		public string Quantity {get;set;}
		public string Description {get;set;}
		public string OwnerId {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, ContactId, AccountId, ParentId, RootAssetId, Product2Id, IsCompetitorProduct, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, IsDeleted, Name, SerialNumber, InstallDate, PurchaseDate, UsageEndDate, Status, Price, Quantity, Description, OwnerId, LastViewedDate, LastReferencedDate FROM Asset";
		}
	}

	public partial class z_FieldDescription__c
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string z_SObjectDescription__c {get;set;}
		public string byteLength__c {get;set;}
		public string calculatedFormula__c {get;set;}
		public string defaultValueFormula__c {get;set;}
		public string digits__c {get;set;}
		public string inlineHelpText__c {get;set;}
		public string isAccessible__c {get;set;}
		public string isAutoNumer__c {get;set;}
		public string isCalculated__c {get;set;}
		public string isCaseSensitive__c {get;set;}
		public string isCreateable__c {get;set;}
		public string isCustom__c {get;set;}
		public string isDefaultedOnCreate__c {get;set;}
		public string isDependentPicklist__c {get;set;}
		public string isDeprecatedAndHidden__c {get;set;}
		public string isExternalID__c {get;set;}
		public string isFilterable__c {get;set;}
		public string isGroupable__c {get;set;}
		public string isHtmlFormatted__c {get;set;}
		public string isIdLookup__c {get;set;}
		public string isNameField__c {get;set;}
		public string isNamePointing__c {get;set;}
		public string isNillable__c {get;set;}
		public string isRestrictedPicklist__c {get;set;}
		public string isSortable__c {get;set;}
		public string isUnique__c {get;set;}
		public string isUpdateable__c {get;set;}
		public string isWriteRequiredMasterRead__c {get;set;}
		public string label__c {get;set;}
		public string length__c {get;set;}
		public string localName__c {get;set;}
		public string name__c {get;set;}
		public string picklistentries__c {get;set;}
		public string precision__c {get;set;}
		public string relationshipName__c {get;set;}
		public string relationshipOrder__c {get;set;}
		public string scale__c {get;set;}
		public string soaptype__c {get;set;}
		public string type__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, z_SObjectDescription__c, byteLength__c, calculatedFormula__c, defaultValueFormula__c, digits__c, inlineHelpText__c, isAccessible__c, isAutoNumer__c, isCalculated__c, isCaseSensitive__c, isCreateable__c, isCustom__c, isDefaultedOnCreate__c, isDependentPicklist__c, isDeprecatedAndHidden__c, isExternalID__c, isFilterable__c, isGroupable__c, isHtmlFormatted__c, isIdLookup__c, isNameField__c, isNamePointing__c, isNillable__c, isRestrictedPicklist__c, isSortable__c, isUnique__c, isUpdateable__c, isWriteRequiredMasterRead__c, label__c, length__c, localName__c, name__c, picklistentries__c, precision__c, relationshipName__c, relationshipOrder__c, scale__c, soaptype__c, type__c FROM z_FieldDescription__c";
		}
	}

	public partial class z_MetaDumpSetting__c
	{
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string name__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, OwnerId, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, name__c FROM z_MetaDumpSetting__c";
		}
	}

	public partial class z_SObjectDescription__c
	{
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string isAccessible__c {get;set;}
		public string isCreateable__c {get;set;}
		public string isCustomSetting__c {get;set;}
		public string isCustom__c {get;set;}
		public string isDeltable__c {get;set;}
		public string isDeprecatedAndHidden__c {get;set;}
		public string isFeedEnabled__c {get;set;}
		public string isMergeable__c {get;set;}
		public string isQueryable__c {get;set;}
		public string isSearchable__c {get;set;}
		public string isUndeletable__c {get;set;}
		public string isUpdateable__c {get;set;}
		public string keyPrefix__c {get;set;}
		public string labelPlural__c {get;set;}
		public string label__c {get;set;}
		public string localName__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, OwnerId, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, isAccessible__c, isCreateable__c, isCustomSetting__c, isCustom__c, isDeltable__c, isDeprecatedAndHidden__c, isFeedEnabled__c, isMergeable__c, isQueryable__c, isSearchable__c, isUndeletable__c, isUpdateable__c, keyPrefix__c, labelPlural__c, label__c, localName__c FROM z_SObjectDescription__c";
		}
	}

	public partial class ObiWan__c
	{
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, OwnerId, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, LastViewedDate, LastReferencedDate FROM ObiWan__c";
		}
	}

	public partial class Campaign
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string ParentId {get;set;}
		public string Type {get;set;}
		public string Status {get;set;}
		public string StartDate {get;set;}
		public string EndDate {get;set;}
		public string ExpectedRevenue {get;set;}
		public string BudgetedCost {get;set;}
		public string ActualCost {get;set;}
		public string ExpectedResponse {get;set;}
		public string NumberSent {get;set;}
		public string IsActive {get;set;}
		public string Description {get;set;}
		public string NumberOfLeads {get;set;}
		public string NumberOfConvertedLeads {get;set;}
		public string NumberOfContacts {get;set;}
		public string NumberOfResponses {get;set;}
		public string NumberOfOpportunities {get;set;}
		public string NumberOfWonOpportunities {get;set;}
		public string AmountAllOpportunities {get;set;}
		public string AmountWonOpportunities {get;set;}
		public string HierarchyNumberOfLeads {get;set;}
		public string HierarchyNumberOfConvertedLeads {get;set;}
		public string HierarchyNumberOfContacts {get;set;}
		public string HierarchyNumberOfResponses {get;set;}
		public string HierarchyNumberOfOpportunities {get;set;}
		public string HierarchyNumberOfWonOpportunities {get;set;}
		public string HierarchyAmountAllOpportunities {get;set;}
		public string HierarchyAmountWonOpportunities {get;set;}
		public string HierarchyNumberSent {get;set;}
		public string HierarchyExpectedRevenue {get;set;}
		public string HierarchyBudgetedCost {get;set;}
		public string HierarchyActualCost {get;set;}
		public string OwnerId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string CampaignMemberRecordTypeId {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, Name, ParentId, Type, Status, StartDate, EndDate, ExpectedRevenue, BudgetedCost, ActualCost, ExpectedResponse, NumberSent, IsActive, Description, NumberOfLeads, NumberOfConvertedLeads, NumberOfContacts, NumberOfResponses, NumberOfOpportunities, NumberOfWonOpportunities, AmountAllOpportunities, AmountWonOpportunities, HierarchyNumberOfLeads, HierarchyNumberOfConvertedLeads, HierarchyNumberOfContacts, HierarchyNumberOfResponses, HierarchyNumberOfOpportunities, HierarchyNumberOfWonOpportunities, HierarchyAmountAllOpportunities, HierarchyAmountWonOpportunities, HierarchyNumberSent, HierarchyExpectedRevenue, HierarchyBudgetedCost, HierarchyActualCost, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, LastViewedDate, LastReferencedDate, CampaignMemberRecordTypeId FROM Campaign";
		}
	}

	public partial class z_SchemaTemp__c
	{
		public string Id {get;set;}
		public string OwnerId {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string fieldname__c {get;set;}
		public string objid__c {get;set;}
		public string objname__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, OwnerId, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, fieldname__c, objid__c, objname__c FROM z_SchemaTemp__c";
		}
	}

	public partial class z_ChildRelationship__c
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string z_ParentSObjectDescription__c {get;set;}
		public string childobjectname__c {get;set;}
		public string fieldname__c {get;set;}
		public string isCascadeDelete__c {get;set;}
		public string z_ChildSObjectDescription__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, z_ParentSObjectDescription__c, childobjectname__c, fieldname__c, isCascadeDelete__c, z_ChildSObjectDescription__c FROM z_ChildRelationship__c";
		}
	}

	public partial class Contract
	{
		public string Id {get;set;}
		public string AccountId {get;set;}
		public string Pricebook2Id {get;set;}
		public string OwnerExpirationNotice {get;set;}
		public string StartDate {get;set;}
		public string EndDate {get;set;}
		public string BillingStreet {get;set;}
		public string BillingCity {get;set;}
		public string BillingState {get;set;}
		public string BillingPostalCode {get;set;}
		public string BillingCountry {get;set;}
		public string BillingLatitude {get;set;}
		public string BillingLongitude {get;set;}
		public string BillingGeocodeAccuracy {get;set;}
		public string BillingAddress {get;set;}
		public string ShippingStreet {get;set;}
		public string ShippingCity {get;set;}
		public string ShippingState {get;set;}
		public string ShippingPostalCode {get;set;}
		public string ShippingCountry {get;set;}
		public string ShippingLatitude {get;set;}
		public string ShippingLongitude {get;set;}
		public string ShippingGeocodeAccuracy {get;set;}
		public string ShippingAddress {get;set;}
		public string ContractTerm {get;set;}
		public string OwnerId {get;set;}
		public string Status {get;set;}
		public string CompanySignedId {get;set;}
		public string CompanySignedDate {get;set;}
		public string CustomerSignedId {get;set;}
		public string CustomerSignedTitle {get;set;}
		public string CustomerSignedDate {get;set;}
		public string SpecialTerms {get;set;}
		public string ActivatedById {get;set;}
		public string ActivatedDate {get;set;}
		public string StatusCode {get;set;}
		public string Description {get;set;}
		public string Name {get;set;}
		public string IsDeleted {get;set;}
		public string ContractNumber {get;set;}
		public string LastApprovedDate {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, AccountId, Pricebook2Id, OwnerExpirationNotice, StartDate, EndDate, BillingStreet, BillingCity, BillingState, BillingPostalCode, BillingCountry, BillingLatitude, BillingLongitude, BillingGeocodeAccuracy, BillingAddress, ShippingStreet, ShippingCity, ShippingState, ShippingPostalCode, ShippingCountry, ShippingLatitude, ShippingLongitude, ShippingGeocodeAccuracy, ShippingAddress, ContractTerm, OwnerId, Status, CompanySignedId, CompanySignedDate, CustomerSignedId, CustomerSignedTitle, CustomerSignedDate, SpecialTerms, ActivatedById, ActivatedDate, StatusCode, Description, Name, IsDeleted, ContractNumber, LastApprovedDate, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, LastViewedDate, LastReferencedDate FROM Contract";
		}
	}

	public partial class Contact
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string MasterRecordId {get;set;}
		public string AccountId {get;set;}
		public string LastName {get;set;}
		public string FirstName {get;set;}
		public string Salutation {get;set;}
		public string Name {get;set;}
		public string OtherStreet {get;set;}
		public string OtherCity {get;set;}
		public string OtherState {get;set;}
		public string OtherPostalCode {get;set;}
		public string OtherCountry {get;set;}
		public string OtherLatitude {get;set;}
		public string OtherLongitude {get;set;}
		public string OtherGeocodeAccuracy {get;set;}
		public string OtherAddress {get;set;}
		public string MailingStreet {get;set;}
		public string MailingCity {get;set;}
		public string MailingState {get;set;}
		public string MailingPostalCode {get;set;}
		public string MailingCountry {get;set;}
		public string MailingLatitude {get;set;}
		public string MailingLongitude {get;set;}
		public string MailingGeocodeAccuracy {get;set;}
		public string MailingAddress {get;set;}
		public string Phone {get;set;}
		public string Fax {get;set;}
		public string MobilePhone {get;set;}
		public string HomePhone {get;set;}
		public string OtherPhone {get;set;}
		public string AssistantPhone {get;set;}
		public string ReportsToId {get;set;}
		public string Email {get;set;}
		public string Title {get;set;}
		public string Department {get;set;}
		public string AssistantName {get;set;}
		public string LeadSource {get;set;}
		public string Birthdate {get;set;}
		public string Description {get;set;}
		public string OwnerId {get;set;}
		public string HasOptedOutOfEmail {get;set;}
		public string HasOptedOutOfFax {get;set;}
		public string DoNotCall {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string LastCURequestDate {get;set;}
		public string LastCUUpdateDate {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string EmailBouncedReason {get;set;}
		public string EmailBouncedDate {get;set;}
		public string IsEmailBounced {get;set;}
		public string PhotoUrl {get;set;}
		public string Jigsaw {get;set;}
		public string JigsawContactId {get;set;}
		public string CleanStatus {get;set;}
		public string Level__c {get;set;}
		public string Languages__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, MasterRecordId, AccountId, LastName, FirstName, Salutation, Name, OtherStreet, OtherCity, OtherState, OtherPostalCode, OtherCountry, OtherLatitude, OtherLongitude, OtherGeocodeAccuracy, OtherAddress, MailingStreet, MailingCity, MailingState, MailingPostalCode, MailingCountry, MailingLatitude, MailingLongitude, MailingGeocodeAccuracy, MailingAddress, Phone, Fax, MobilePhone, HomePhone, OtherPhone, AssistantPhone, ReportsToId, Email, Title, Department, AssistantName, LeadSource, Birthdate, Description, OwnerId, HasOptedOutOfEmail, HasOptedOutOfFax, DoNotCall, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, LastCURequestDate, LastCUUpdateDate, LastViewedDate, LastReferencedDate, EmailBouncedReason, EmailBouncedDate, IsEmailBounced, PhotoUrl, Jigsaw, JigsawContactId, CleanStatus, Level__c, Languages__c FROM Contact";
		}
	}

	public partial class Case
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string CaseNumber {get;set;}
		public string ContactId {get;set;}
		public string AccountId {get;set;}
		public string AssetId {get;set;}
		public string ParentId {get;set;}
		public string SuppliedName {get;set;}
		public string SuppliedEmail {get;set;}
		public string SuppliedPhone {get;set;}
		public string SuppliedCompany {get;set;}
		public string Type {get;set;}
		public string Status {get;set;}
		public string Reason {get;set;}
		public string Origin {get;set;}
		public string Subject {get;set;}
		public string Priority {get;set;}
		public string Description {get;set;}
		public string IsClosed {get;set;}
		public string ClosedDate {get;set;}
		public string IsEscalated {get;set;}
		public string OwnerId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string EngineeringReqNumber__c {get;set;}
		public string SLAViolation__c {get;set;}
		public string Product__c {get;set;}
		public string PotentialLiability__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, CaseNumber, ContactId, AccountId, AssetId, ParentId, SuppliedName, SuppliedEmail, SuppliedPhone, SuppliedCompany, Type, Status, Reason, Origin, Subject, Priority, Description, IsClosed, ClosedDate, IsEscalated, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, EngineeringReqNumber__c, SLAViolation__c, Product__c, PotentialLiability__c FROM Case";
		}
	}

	public partial class Opportunity
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string AccountId {get;set;}
		public string IsPrivate {get;set;}
		public string Name {get;set;}
		public string Description {get;set;}
		public string StageName {get;set;}
		public string Amount {get;set;}
		public string Probability {get;set;}
		public string ExpectedRevenue {get;set;}
		public string TotalOpportunityQuantity {get;set;}
		public string CloseDate {get;set;}
		public string Type {get;set;}
		public string NextStep {get;set;}
		public string LeadSource {get;set;}
		public string IsClosed {get;set;}
		public string IsWon {get;set;}
		public string ForecastCategory {get;set;}
		public string ForecastCategoryName {get;set;}
		public string CampaignId {get;set;}
		public string HasOpportunityLineItem {get;set;}
		public string Pricebook2Id {get;set;}
		public string OwnerId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastActivityDate {get;set;}
		public string FiscalQuarter {get;set;}
		public string FiscalYear {get;set;}
		public string Fiscal {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string ContractId {get;set;}
		public string HasOpenActivity {get;set;}
		public string HasOverdueTask {get;set;}
		public string DeliveryInstallationStatus__c {get;set;}
		public string TrackingNumber__c {get;set;}
		public string OrderNumber__c {get;set;}
		public string CurrentGenerators__c {get;set;}
		public string MainCompetitors__c {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, AccountId, IsPrivate, Name, Description, StageName, Amount, Probability, ExpectedRevenue, TotalOpportunityQuantity, CloseDate, Type, NextStep, LeadSource, IsClosed, IsWon, ForecastCategory, ForecastCategoryName, CampaignId, HasOpportunityLineItem, Pricebook2Id, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastActivityDate, FiscalQuarter, FiscalYear, Fiscal, LastViewedDate, LastReferencedDate, ContractId, HasOpenActivity, HasOverdueTask, DeliveryInstallationStatus__c, TrackingNumber__c, OrderNumber__c, CurrentGenerators__c, MainCompetitors__c FROM Opportunity";
		}
	}

	public partial class Pricebook2
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string Name {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string IsActive {get;set;}
		public string Description {get;set;}
		public string IsStandard {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, Name, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, IsActive, Description, IsStandard FROM Pricebook2";
		}
	}

	public partial class Idea
	{
		public string Id {get;set;}
		public string IsDeleted {get;set;}
		public string Title {get;set;}
		public string RecordTypeId {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string LastViewedDate {get;set;}
		public string LastReferencedDate {get;set;}
		public string CommunityId {get;set;}
		public string Body {get;set;}
		public string NumComments {get;set;}
		public string VoteScore {get;set;}
		public string VoteTotal {get;set;}
		public string Categories {get;set;}
		public string Status {get;set;}
		public string LastCommentDate {get;set;}
		public string LastCommentId {get;set;}
		public string ParentIdeaId {get;set;}
		public string IsHtml {get;set;}
		public string IsMerged {get;set;}
		public string AttachmentName {get;set;}
		public string AttachmentContentType {get;set;}
		public string AttachmentLength {get;set;}
		public string AttachmentBody {get;set;}
		public string CreatorFullPhotoUrl {get;set;}
		public string CreatorSmallPhotoUrl {get;set;}
		public string CreatorName {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, IsDeleted, Title, RecordTypeId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, LastViewedDate, LastReferencedDate, CommunityId, Body, NumComments, VoteScore, VoteTotal, Categories, Status, LastCommentDate, LastCommentId, ParentIdeaId, IsHtml, IsMerged, AttachmentName, AttachmentContentType, AttachmentLength, AttachmentBody, CreatorFullPhotoUrl, CreatorSmallPhotoUrl, CreatorName FROM Idea";
		}
	}

	public partial class PricebookEntry
	{
		public string Id {get;set;}
		public string Name {get;set;}
		public string Pricebook2Id {get;set;}
		public string Product2Id {get;set;}
		public string UnitPrice {get;set;}
		public string IsActive {get;set;}
		public string UseStandardPrice {get;set;}
		public string CreatedDate {get;set;}
		public string CreatedById {get;set;}
		public string LastModifiedDate {get;set;}
		public string LastModifiedById {get;set;}
		public string SystemModstamp {get;set;}
		public string ProductCode {get;set;}
		public string IsDeleted {get;set;}

		public static string GetSelectQuery()
		{
			return "SELECT Id, Name, Pricebook2Id, Product2Id, UnitPrice, IsActive, UseStandardPrice, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, ProductCode, IsDeleted FROM PricebookEntry";
		}
	}

}

