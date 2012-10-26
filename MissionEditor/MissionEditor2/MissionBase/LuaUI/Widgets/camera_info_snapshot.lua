function widget:GetInfo()
  return {
    name      = "Camera Info Snapshot",
    desc      = "Prints your camera position and rotation with F10",
    author    = "KingRaptor (L.J. Lim)",
    date      = "2012.10.27",
    license   = "Public Domain",
    layer     = 0,
    enabled   = true,
  }
end

function widget:KeyPress(key)
  if key == 291 then	-- F10
    local cam = Spring.GetCameraState()
    Spring.Echo(cam.px, cam.py, cam.pz, math.deg(cam.rx), math.deg(cam.ry))
  end
end