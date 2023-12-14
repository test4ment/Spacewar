﻿namespace Spacewar;

public interface ICommand
{
    public void Execute();
}

public interface IRotateable
{
    public int angle { get; set; }
    public int angle_velocity { get; }
}

public class RotateCommand : ICommand
{
    private readonly IRotateable rotating_object;

    public RotateCommand(IRotateable rotating_object)
    {
        this.rotating_object = rotating_object;
    }

    public void Execute()
    {
        rotating_object.angle = ((rotating_object.angle + rotating_object.angle_velocity) % 360) ;
    }
}
