
cbuffer OutlineBlurVars : register(b0)
{
    float4 OutlineColour;
    int StepDirectionX;
    int StepDirectionY;
    int Stage; // 0 = horizontal, 1 = vertical
    int Width;
}

Texture2D MaskTex : register(t0);
Texture2D BlurTex : register(t1); // result from stage 0, only use in stage 1


float4 main(float4 pos : SV_POSITION) : SV_TARGET
{
    float alpha = 1;
    if (Stage == 1)
    {
        int3 ssloc = int3(pos.xy, 0);
        float4 px  = MaskTex.Load(ssloc);
        float4 px1 = MaskTex.Load(ssloc + int3( 1,  1, 0));
        float4 px2 = MaskTex.Load(ssloc + int3( 1, -1, 0));
        float4 px3 = MaskTex.Load(ssloc + int3(-1,  1, 0));
        float4 px4 = MaskTex.Load(ssloc + int3(-1, -1, 0));
        alpha = 1 - saturate((px.r + px1.r + px2.r + px3.r + px4.r) * 0.2);
        if (alpha <= 0) discard;
    }

    int width = min(Width, 8);
    float tot = 0;
    int2 ss = int2(pos.xy);
    int2 step = int2(StepDirectionX, StepDirectionY);
    int start = -width;
    for (int i = start; i <= width; i++)
    {
        int3 sp = int3(max(ss + step * i, int2(0, 0)), 0);
        float4 sv = (Stage == 0) ? MaskTex.Load(sp) : BlurTex.Load(sp);
        tot += sv.r;
    }
    float f = saturate(tot / ((float)width));

    return (Stage == 0) ? float4(f, f, f, 1) : float4(OutlineColour.rgb, OutlineColour.a * f * alpha);
}
