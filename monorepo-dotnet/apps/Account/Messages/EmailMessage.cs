namespace Account.Messages;

public record EmailMessage(string To, string Subject, string Body);