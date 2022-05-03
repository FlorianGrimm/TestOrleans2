namespace Replacement.Contracts.Entity;
partial record ToDoPK {
    public static bool Parse(string value, out ToDoPK result) {
        Guid projectId;
        Guid toDoId;
        if ((value.Length == 73)
            && (value[36] == '-')
            && Guid.TryParse(
                value.AsSpan(0, 36),
                out projectId
            )
            && Guid.TryParse(
                value.AsSpan(37, 36),
                out toDoId
            )) {
            result = new ToDoPK(projectId, toDoId);
            return true;
        } else if ((value.Length == 77)
            && (value[40]=='-')
            && Guid.TryParse(
                value.AsSpan(0, 40),
                out projectId
            )
            && Guid.TryParse(
                value.AsSpan(41, 81),
                out toDoId
            )) {
            result = new ToDoPK(projectId, toDoId);
            return true;
        } else {
            result = new ToDoPK(Guid.Empty, Guid.Empty);
            return false;
        }

    }
    public override string ToString() {
        return $"{this.ProjectId}-{this.ToDoId}";
    }
}
