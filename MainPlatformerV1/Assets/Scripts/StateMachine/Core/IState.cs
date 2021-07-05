using Aarthificial.Reanimation.Common;

public interface IState {
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}