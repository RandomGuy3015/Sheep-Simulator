using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



public interface IMovable : IGameObject
{
    public float Speed { get; set; }
    public float SpeedSprint { get; set; }
    public int SituationBehaviour { get; set; }
    public Vector2 Direction { get; set; }
    public float Facing { get; set; }
    public List<Vector2> Path { get; set; }
    void Animate(GameTime gameTime);
    void MoveStep(bool sprinting);
    void SetDestination(int range, bool randomDestination);
    Vector2 Center();
    Vector2 Destination();
}
