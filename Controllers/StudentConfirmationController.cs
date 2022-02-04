using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EthosExtensionApi.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EthosExtensionApi.Controllers;

[ApiController]
[Route("api/student-confirmations")]
public class StudentConfirmationController : ControllerBase
{
    private readonly ILogger<StudentConfirmationController> _logger;
    private readonly EthosExtensionContext _dbContext;
    private readonly IConfiguration _config;

    public StudentConfirmationController(ILogger<StudentConfirmationController> logger, EthosExtensionContext dbContext, IConfiguration config)
    {
        _logger = logger;
        _dbContext = dbContext;
        _config = config;
    }

    [HttpGet(Name = "GetStudentConfirmation")]
    //public IEnumerable<StudentConfirmation> Get(string? criteria = "")
    public Object Get(string? criteria = "")
    {
        JsonDocument? jsonCriteria = null;
        string? person = null;
        string? code = null;

        string auth = Request.Headers["Authorization"].ToString();
        string api_key = _config.GetSection("ApiKeys").GetSection("CMAP").Value;

        if (auth != $"Bearer {api_key}")
        {
            return StatusCode(403);
        }

        if (!string.IsNullOrWhiteSpace(criteria))
        {
            jsonCriteria = JsonDocument.Parse(criteria);

            if (jsonCriteria.RootElement.TryGetProperty("person", out JsonElement criteriaPerson))
            {
                person = jsonCriteria.RootElement.GetProperty("person").GetString();
                if (person != null && !Regex.Match(person, @"^([0-9]{7})$").Success)
                {
                    person = "NOT MATCHED";
                }
            }

            if (jsonCriteria.RootElement.TryGetProperty("code", out JsonElement criteriaCode))
            {
                code = jsonCriteria.RootElement.GetProperty("code").GetString();
                if (code != null && !Regex.Match(code, @"^([0-9A-Z]{1,8})$").Success)
                {
                    code = "NOT MATCHED";
                }
            }

        }

        string query = @"
SELECT MAILING_ID AS [PersonId]
  , MAILING_CORR_RECEIVED AS [Code]
  , MAILING_CORR_RECEIVED_DATE AS [ConfirmationDate]
FROM CH_CORR
INNER JOIN VALS AS v ON v.VAL_INTERNAL_CODE = CH_CORR.MAILING_CORR_RECVD_STATUS
  AND v.VALCODE_ID = 'CORR.STATUSES'
WHERE (CH_CORR.MAILING_CORR_RECEIVED LIKE 'CONF%FA' OR CH_CORR.MAILING_CORR_RECEIVED LIKE 'CONF%SP')
  AND v.VAL_ACTION_CODE_1 IN (1, 0)
";

        if (!string.IsNullOrWhiteSpace(person))
        {
            query += " AND CH_CORR.MAILING_ID = '" + person + "'";
        }

        if (!string.IsNullOrWhiteSpace(code))
        {
            query += " AND CH_CORR.MAILING_CORR_RECEIVED = '" + code + "'";
        }

        return Ok(_dbContext.Set<StudentConfirmation>().FromSqlRaw(query).ToArray());
    }
}