
namespace EnterpriseMcpServer;

/// <summary>
/// Contains application-wide constants, error messages, success messages, log messages, and Dataverse configuration.
/// </summary>
public static class Constants
{

    /// <summary>
    /// Contains all error messages used throughout the application.
    /// </summary>
    public struct ErrorMessages
    {
        // Validation errors
        /// <summary>Error message when name field is required but missing.</summary>
        public const string NameIsRequired = "Name is required.";

        /// <summary>Error message when email field is required but missing.</summary>
        public const string EmailIsRequired = "Email is required.";

        /// <summary>Error message when email parameter is required but missing.</summary>
        public const string EmailParameterIsRequired = "Email parameter is required";

        /// <summary>Error message when request body is required but missing.</summary>
        public const string RequestBodyIsRequired = "Request body is required.";

        /// <summary>Error message when ID format is invalid.</summary>
        public const string InvalidIdFormat = "Invalid ID format";

        /// <summary>Error message when name exceeds maximum length. Format parameter: {0} = max length.</summary>
        public const string NameTooLong = "Name exceeds the maximum allowed length of {0} characters.";

        /// <summary>Error message when email exceeds maximum length. Format parameter: {0} = max length.</summary>
        public const string EmailTooLong = "Email exceeds the maximum allowed length of {0} characters.";

        /// <summary>Error message when message exceeds maximum length. Format parameter: {0} = max length.</summary>
        public const string MessageTooLong = "Message exceeds the maximum allowed length of {0} characters.";

        /// <summary>Error message when source exceeds maximum length. Format parameter: {0} = max length.</summary>
        public const string SourceTooLong = "Source exceeds the maximum allowed length of {0} characters.";

        // Not found errors
        /// <summary>Error message when enquiry record is not found.</summary>
        public const string EnquiryNotFound = "Enquiry not found";

        // Connection errors
        /// <summary>Error message when Dataverse connection string is missing from configuration.</summary>
        public const string DataverseConnectionStringMissing = "DataverseConnectionString is missing from configuration.";

        /// <summary>Error message when unable to connect to Microsoft Dataverse.</summary>
        public const string UnableToConnectToDataverse = "Unable to connect to Microsoft Dataverse.";

        // General errors
        /// <summary>Error message for unexpected errors during record creation.</summary>
        public const string UnexpectedErrorCreating = "An unexpected error occurred while creating the record.";

        /// <summary>Error message for unexpected errors during record update.</summary>
        public const string UnexpectedErrorUpdating = "An unexpected error occurred while updating the record.";

        /// <summary>Error message when retrieving an enquiry fails. Format parameter: {0} = error message.</summary>
        public const string ErrorRetrievingEnquiry = "Error retrieving enquiry: {0}";

        /// <summary>Error message when retrieving enquiries fails. Format parameter: {0} = error message.</summary>
        public const string ErrorRetrievingEnquiries = "Error retrieving enquiries: {0}";
    }

    /// <summary>
    /// Contains all success messages returned to API clients.
    /// </summary>
    public struct SuccessMessages
    {
        /// <summary>Success message when an enquiry is created.</summary>
        public const string EnquiryCreatedSuccessfully = "Dataverse record created successfully.";

        /// <summary>Success message when an enquiry is updated.</summary>
        public const string EnquiryUpdatedSuccessfully = "Dataverse record updated successfully.";

        /// <summary>Success message when an enquiry is retrieved.</summary>
        public const string EnquiryRetrievedSuccessfully = "Enquiry retrieved successfully";

        /// <summary>Success message format when enquiries are retrieved. Format parameter: {0} = count.</summary>
        public const string EnquiriesRetrievedFormat = "Retrieved {0} enquiries";

        /// <summary>Success message format when enquiries are retrieved by email. Format parameters: {0} = count, {1} = email.</summary>
        public const string EnquiriesRetrievedForEmailFormat = "Retrieved {0} enquiries for {1}";
    }

    /// <summary>
    /// Contains all structured log messages for application logging.
    /// </summary>
    public struct LogMessages
    {
        // Information logs
        /// <summary>Log message when an enquiry record is created.</summary>
        public const string CreatedDataverseEnquiryRecord = "Created Dataverse enquiry record. RecordId: {RecordId}";

        /// <summary>Log message when an enquiry record is updated.</summary>
        public const string UpdatedDataverseEnquiryRecord = "Updated Dataverse enquiry record. RecordId: {RecordId}";

        /// <summary>Log message when an enquiry record is deleted.</summary>
        public const string DeletedDataverseEnquiryRecord = "Deleted Dataverse enquiry record. RecordId: {RecordId}";

        /// <summary>Log message when an enquiry record is retrieved.</summary>
        public const string RetrievedDataverseEnquiryRecord = "Retrieved Dataverse enquiry record. RecordId: {RecordId}";

        /// <summary>Log message when multiple enquiry records are retrieved.</summary>
        public const string RetrievedEnquiryRecords = "Retrieved {Count} enquiry records from Dataverse";

        /// <summary>Log message when enquiry records are retrieved for a specific email.</summary>
        public const string RetrievedEnquiryRecordsForEmail = "Retrieved {Count} enquiry records for email: {Email}";

        // Function processing logs
        /// <summary>Log message when processing a GetEnquiry request.</summary>
        public const string ProcessingGetEnquiryRequest = "Processing GetEnquiry request for ID: {Id}";

        /// <summary>Log message when processing a GetEnquiries request.</summary>
        public const string ProcessingGetEnquiriesRequest = "Processing GetEnquiries request";

        /// <summary>Log message when processing a GetEnquiriesByEmail request.</summary>
        public const string ProcessingGetEnquiriesByEmailRequest = "Processing GetEnquiriesByEmail request for: {Email}";

        // Warning logs
        /// <summary>Warning log message when an enquiry record is not found.</summary>
        public const string EnquiryRecordNotFound = "Enquiry record not found. RecordId: {RecordId}";

        /// <summary>Warning log message when validation fails.</summary>
        public const string ValidationFailed = "Validation failed for create/update request. Errors: {Errors}";

        // Error logs
        /// <summary>Error log message when Dataverse ServiceClient is not ready.</summary>
        public const string DataverseServiceClientNotReady = "Dataverse ServiceClient is not ready. Error: {Error}";

        /// <summary>Error log message when retrieving an enquiry by ID fails.</summary>
        public const string ErrorRetrievingEnquiryWithId = "Error retrieving enquiry with ID: {Id}";

        /// <summary>Error log message when retrieving enquiries fails.</summary>
        public const string ErrorRetrievingEnquiries = "Error retrieving enquiries";

        /// <summary>Error log message when retrieving enquiries by email fails.</summary>
        public const string ErrorRetrievingEnquiriesForEmail = "Error retrieving enquiries for email: {Email}";

        /// <summary>Error log message when creating a Dataverse record fails.</summary>
        public const string FailedToCreateDataverseRecord = "Failed to create Dataverse record.";

        /// <summary>Error log message when updating a Dataverse record fails.</summary>
        public const string FailedToUpdateDataverseRecord = "Failed to update Dataverse record. RecordId: {RecordId}";
    }

    /// <summary>
    /// Contains Microsoft Dataverse table and column configuration.
    /// </summary>
    public struct Dataverse
    {
        /// <summary>The Dataverse table name for enquiries.</summary>
        public const string TableName = "crf51_sitecoreenquiry";

        /// <summary>Default source value when not specified.</summary>
        public const string DefaultSource = "SitecoreAI";

        /// <summary>
        /// Contains Dataverse table column names.
        /// </summary>
        public struct Columns
        {
            /// <summary>Column name for the enquiry name/title.</summary>
            public const string NewColumn = "crf51_newcolumn";

            /// <summary>Column name for the email address.</summary>
            public const string Email = "crf51_email";

            /// <summary>Column name for the message content.</summary>
            public const string Message = "crf51_message";

            /// <summary>Column name for the source system.</summary>
            public const string Source = "crf51_source";

            /// <summary>Column name for the creation timestamp.</summary>
            public const string CreatedOn = "createdon";

            /// <summary>Column name for the last modification timestamp.</summary>
            public const string ModifiedOn = "modifiedon";
        }

        /// <summary>
        /// Contains maximum field length constraints for Dataverse columns.
        /// </summary>
        public struct MaxLength
        {
            /// <summary>Maximum length for the name field.</summary>
            public const int Name = 100;

            /// <summary>Maximum length for the email field.</summary>
            public const int Email = 100;

            /// <summary>Maximum length for the message field.</summary>
            public const int Message = 100;

            /// <summary>Maximum length for the source field.</summary>
            public const int Source = 100;
        }
    }
}