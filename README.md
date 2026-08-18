# SoftRast3D — CPU-Based 3D Rendering Engine

A **software 3D rendering engine** built from scratch in C# that implements the classic graphics pipeline entirely on the CPU. No DirectX, OpenGL, or GPU APIs — everything is hand-coded, making it an ideal **educational tool** for understanding how 3D graphics work at the fundamental level.

## Features

- **Full Graphics Pipeline** — Model, view, and projection matrix transformations
- **Rasterization** — Active Edge Table (AET) scanline algorithm with z-buffering
- **Shading Modes** — Flat, Gouraud, and Phong shading with per-pixel interpolation
- **Lighting System** — Phong illumination model with ambient, diffuse, and specular terms
  - Point lights
  - Spotlights with angular attenuation
  - Multiple simultaneous lights
- **Dynamic Objects** — Parametric mesh generators (Cylinder, Cone, Icosphere)
- **Camera System** — Static, third-person, and following camera modes
- **Visual Effects** — Linear distance fog, real-time animation
- **Geometry** — Vertex transformations with normal mapping, face culling
- **Performance** — DirectBitmap pinned-memory optimization for pixel writing

## Architecture

```
SoftRast3D/
├── Devices/
│   ├── Camera.cs          — View matrix (look-at) construction
│   ├── Device.cs          — Main rendering pipeline
│   └── DirectBitmap.cs    — Optimized pixel buffer (GCHandle pinning)
├── Geometry/
│   ├── Vertex.cs          — Vertex data + transform operations
│   ├── Face.cs            — Triangle primitive (3 vertices + normals)
│   └── Mesh.cs            — Mesh container + model matrix transforms
├── Lights/
│   ├── Light.cs           — Point/spotlight definitions
│   ├── Fog.cs             — Linear fog effect
│   └── Shadings.cs        — Phong model, rasterization, barycentric coords
├── Shapes/
│   ├── Cube.cs            — Static cube mesh
│   ├── Cylinder.cs        — Parametric cylinder with end caps
│   ├── Cone.cs            — Parametric cone
│   ├── Floor.cs           — Flat quad
│   └── ShapesGenerators.cs — Icosphere (subdivided icosahedron)
└── Form1.cs               — UI & animation loop
```

## How It Works

### Rendering Pipeline
1. **Transformation** — Apply model, view, and projection matrices to all vertices
2. **Culling** — Remove back-facing triangles (normal·viewDir ≤ 0)
3. **Clipping** — Skip triangles behind the near plane
4. **Rasterization** — Fill each triangle using scanline algorithm
5. **Depth Test** — Z-buffer resolves occlusion
6. **Shading** — Phong lighting with selected mode (Flat/Gouraud/Phong)
7. **Fog** — Optional linear distance fog blending

### Shading Modes

| Mode | Performance | Quality | How It Works |
|------|-------------|---------|--------------|
| **Flat** | Very Fast | Low | Compute lighting once per triangle |
| **Gouraud** | Fast | Medium | Compute lighting per-vertex, interpolate |
| **Phong** | Slower | High | Interpolate normals, compute per-pixel |

### Illumination Model
Uses the classic **Phong lighting equation**:
```
I = Ka·color + Σ[ Kd·(N·L)·color + Ks·(R·V)^M ]
```
- **Ka** — Ambient coefficient
- **Kd** — Diffuse coefficient  
- **Ks** — Specular coefficient
- **M** — Specular exponent (shininess)
- **Spotlight factor** — `cos^P(angle)` for cone tightness

## Controls

| Feature | Options |
|---------|---------|
| **Camera** | Static (bird's-eye), Third-Person, Following |
| **Shading** | Flat, Gouraud, Phong |
| **Lighting** | Toggle Global Light, Spotlight, Dynamic Light |
| **Effects** | Toggle Fog, Pause Animation |

## Scene Description

The default scene includes:
- **Animated Cube** — Center stage, rotates and moves horizontally
- **Rotating Sphere** — Demonstrates smooth mesh generation
- **Cylinder Light** — Spotlight with animated direction
- **Floor Plane** — Reference ground
- **Three Light Sources** — Global (static), rotating (attached to cylinder), dynamic (follows camera)

## Educational Value

This project demonstrates:
- **Matrix math** — Transform operations, normal transformation
- **Geometric algorithms** — Barycentric coordinates, triangle rasterization
- **Classical graphics** — How GPUs implement the rendering pipeline
- **Performance optimization** — DirectBitmap, edge table rasterization
- **Lighting & shading** — Phong model, spotlight calculations, fog
- **3D mathematics** — Vertex normals, cross products, vector interpolation

## Performance Considerations

- Renders at ~100 FPS on modern hardware for the default scene
- Uses **DirectBitmap** (GCHandle pinning) to avoid repeated Bitmap locks
- **Scanline algorithm** minimizes redundant calculations
- Z-buffer prevents overdraw computation for occluded pixels

## Future Enhancements

- Texture mapping (UV coordinates)
- Smooth normal interpolation (Phong shading per-pixel optimization)
- Directional lights (parallel projection)
- Model loading (OBJ/FBX)
- Depth-of-field post-processing
- SIMD vectorization for matrix operations

## Author

Built as a **computer graphics coursework project** to deeply understand the rendering pipeline.

## License

[Add your license here]

---

**Learn how 3D graphics really work — no GPU magic, just math and algorithms.**
