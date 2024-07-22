using Common.Enums;
using Stateless;

namespace BLL.StateMachine;

public class OrderStateMachine
{
    private readonly StateMachine<OrderStatus, OrderEvent> _stateMachine;

    public OrderStateMachine(OrderStatus initialState)
    {
        _stateMachine = new StateMachine<OrderStatus, OrderEvent>(initialState);

        _stateMachine.Configure(OrderStatus.CREATED)
            .Permit(OrderEvent.PROCESS, OrderStatus.PREPARED)
            .Permit(OrderEvent.CANCEL, OrderStatus.CANCELLED);

        _stateMachine.Configure(OrderStatus.PREPARED)
            .Permit(OrderEvent.SHIP, OrderStatus.SHIPPED);

        _stateMachine.Configure(OrderStatus.SHIPPED)
            .Permit(OrderEvent.DELIVER, OrderStatus.SUCCEEDED);
    }

    public OrderStatus CurrentState => _stateMachine.State;

    public void Fire(OrderEvent orderEvent)
    {
        _stateMachine.Fire(orderEvent);
    }
}