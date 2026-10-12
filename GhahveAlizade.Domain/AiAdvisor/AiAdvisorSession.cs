using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.AiAdvisor;

public class AiAdvisorSession : BaseEntity
{
	public string UserId { get; set; } = default!;

	public bool IsClosed { get; set; }
	public DateTime? ClosedAtUtc { get; set; }

	public ICollection<AiAdvisorMessage> Messages { get; set; } = new List<AiAdvisorMessage>();
}

public class AiAdvisorMessage : BaseEntity
{
	public long SessionId { get; set; }
	public AiAdvisorSession Session { get; set; } = default!;

	public string Role { get; set; } = default!;     // "user" | "model"
	public string Content { get; set; } = default!;
}
