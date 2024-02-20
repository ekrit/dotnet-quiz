namespace Quiz.Core.Interfaces;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create(Enums.UnitOfWorkMode mode = Enums.UnitOfWorkMode.ReadOnly);
}