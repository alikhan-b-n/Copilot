using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.RecordService;

namespace Copilot.Altegio.Api.Interfaces;

public interface IRecordService
{
    /// <summary>
    /// Retrieves a collection of records asynchronously.
    /// </summary>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="page">The page number.</param>
    /// <param name="count">The number of records per page.</param>
    /// <param name="staffId">The ID of the staff member. Use this parameter to retrieve records for a specific staff member.</param>
    /// <param name="clientId">The ID of the client. Use this parameter to retrieve records for a specific client.</param>
    /// <param name="startDate">The start date of the session (filter by session date). Use this parameter to retrieve records for sessions starting from a specific date.</param>
    /// <param name="endDate">The end date of the session (filter by session date). Use this parameter to retrieve records for sessions ending before a specific date.</param>
    /// <param name="cStartDate">The start date of record creation (filter by record creation date). Use this parameter to retrieve records created starting from a specific date.</param>
    /// <param name="cEndDate">The end date of record creation (filter by record creation date). Use this parameter to retrieve records created before a specific date.</param>
    /// <param name="changedAfter">The date and time of record modification/creation. Use this parameter to retrieve records created/modified starting from a specific date and time.</param>
    /// <param name="changedBefore">The date and time of record modification/creation. Use this parameter to retrieve records created/modified before a specific date and time.</param>
    /// <param name="includeConsumables">A flag indicating whether to include consumables in the records.</param>
    /// <param name="includeFinanceTransactions">A flag indicating whether to include financial transactions in the records.</param>
    /// <returns>An awaitable task that represents the asynchronous operation. The task result contains a response object of type AltegioResponse<RecordResponse>.</returns>
    Task<AltegioResponse<RecordResponse>> GetRecordsAsync(
        int companyId,
        int? page = null,
        int? count = null,
        int? staffId = null,
        int? clientId = null,
        string startDate = null,
        string endDate = null,
        string cStartDate = null,
        string cEndDate = null,
        string changedAfter = null,
        string changedBefore = null,
        int includeConsumables = 0,
        int includeFinanceTransactions = 0);

    /// <summary>
    /// Retrieves a single record asynchronously.
    /// </summary>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="recordId">The ID of the record.</param>
    /// <returns>An awaitable task that represents the asynchronous operation. The task result contains a response object of type AltegioSingleResponse<RecordResponse>.</returns>
    Task<AltegioSingleResponse<RecordResponse>> GetRecordAsync(int companyId, int recordId);

    Task<AltegioSingleResponse<RecordResponse>> EditRecordAsync(int companyId, int recordId, RecordPut record);

    Task DeleteRecordAsync(int companyId, int recordId);
}
