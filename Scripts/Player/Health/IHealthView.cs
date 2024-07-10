using System;

public interface IHealthView
{
    public void RenderHealth(float healthRatio);

    public void Disable();
}