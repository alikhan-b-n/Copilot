using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.BookformService;
using Copilot.Altegio.Api.Models.RecordService;

namespace Copilot.Altegio.Api.Interfaces;

public interface IBookformService
{
    Task<AltegioSingleResponse<BookServicesResponse>> GetBookServicesAsync(int companyId,
        int? staffId = null, DateTimeOffset? date = null, int[] serviceIds = null);

    Task<AltegioSingleResponse<GetBookStuffSeancesResponse>> GetBookStuffSeances(int companyId,
        int staffId, DateTimeOffset? date = null, int[] serviceIds = null);

    Task<AltegioResponse<SeanceResponse>> GetBookTimes(
        int companyId, int staffId, DateTimeOffset date, int[] serviceIds = null);

    Task<AltegioMetaResponse<BookCheckMeta>> BookCheck(int companyId,
        BookCheckParameter parameter);

    Task<AltegioResponse<BookRecordResult>> BookRecord(int companyId, BookRecordParameter parameter);
}
