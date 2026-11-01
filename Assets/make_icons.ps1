Add-Type -AssemblyName System.Drawing

function Create-Icon {
    param([string]$name, [scriptblock]$drawAction)
    $bmp = New-Object System.Drawing.Bitmap 32, 32
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)
    
    $goldBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(212, 175, 55))
    $goldPen = New-Object System.Drawing.Pen $goldBrush, 2
    $whiteBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::White)
    $whitePen = New-Object System.Drawing.Pen $whiteBrush, 2
    
    & $drawAction $g $goldBrush $goldPen $whiteBrush $whitePen
    
    $path = "c:\Users\class\Downloads\RestaurantePOS\RestaurantePOS\Assets\.png"
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose()
    $bmp.Dispose()
}

# 1. Pedidos (Clipboard / Order list)
Create-Icon "icon_pedidos" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawRectangle($gp, 6, 6, 20, 22)
    $g.FillRectangle($gb, 11, 3, 10, 5)
    $g.DrawLine($wp, 10, 13, 22, 13)
    $g.DrawLine($wp, 10, 18, 22, 18)
    $g.DrawLine($wp, 10, 23, 18, 23)
}

# 2. Productos (Cloche / Plate / Fork & Spoon)
Create-Icon "icon_productos" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawArc($gp, 4, 8, 24, 18, 180, 180)
    $g.DrawLine($gp, 3, 17, 29, 17)
    $g.FillEllipse($gb, 14, 5, 4, 4)
    $g.DrawLine($wp, 6, 22, 26, 22)
    $g.DrawLine($wp, 9, 25, 23, 25)
}

# 3. Clientes (Users / People)
Create-Icon "icon_clientes" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawEllipse($gp, 11, 4, 10, 10)
    $g.DrawArc($gp, 4, 15, 24, 18, 180, 180)
    $g.DrawLine($wp, 4, 24, 28, 24)
    $g.DrawEllipse($wp, 4, 8, 6, 6)
    $g.DrawEllipse($wp, 22, 8, 6, 6)
}

# 4. Empleados (Chef / Badge)
Create-Icon "icon_empleados" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawEllipse($gp, 7, 6, 8, 8)
    $g.DrawEllipse($gp, 17, 6, 8, 8)
    $g.DrawEllipse($gp, 12, 3, 8, 8)
    $g.DrawRectangle($wp, 8, 14, 16, 5)
    $g.DrawArc($gp, 6, 18, 20, 14, 180, 180)
    $g.DrawLine($wp, 6, 25, 26, 25)
}

# 5. Proveedores (Delivery Truck)
Create-Icon "icon_proveedores" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawRectangle($gp, 4, 8, 15, 12)
    $g.DrawPolygon($gp, @([System.Drawing.Point]::new(19,12), [System.Drawing.Point]::new(27,12), [System.Drawing.Point]::new(27,20), [System.Drawing.Point]::new(19,20)))
    $g.FillEllipse($gb, 7, 18, 6, 6)
    $g.FillEllipse($gb, 21, 18, 6, 6)
    $g.DrawLine($wp, 7, 12, 15, 12)
}

# 6. Pagos (Credit Card)
Create-Icon "icon_pagos" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawRectangle($gp, 4, 7, 24, 17)
    $g.FillRectangle($gb, 4, 11, 24, 4)
    $g.DrawRectangle($wp, 7, 18, 5, 3)
    $g.DrawLine($wp, 15, 19, 24, 19)
}

# 7. Reportes (Analytics / Charts)
Create-Icon "icon_reportes" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawLine($gp, 5, 5, 5, 27)
    $g.DrawLine($gp, 5, 27, 27, 27)
    $g.FillRectangle($gb, 8, 18, 4, 8)
    $g.FillRectangle($wb, 14, 12, 4, 14)
    $g.FillRectangle($gb, 20, 7, 4, 19)
}

# 8. Logout (Door / Exit)
Create-Icon "icon_logout" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawRectangle($gp, 6, 4, 12, 24)
    $g.FillEllipse($gb, 14, 15, 2, 2)
    $g.DrawLine($wp, 14, 16, 26, 16)
    $g.DrawLine($wp, 22, 11, 26, 16)
    $g.DrawLine($wp, 22, 21, 26, 16)
}

# 9. Add (+)
Create-Icon "icon_add" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawEllipse($gp, 4, 4, 24, 24)
    $g.DrawLine($wp, 16, 10, 16, 22)
    $g.DrawLine($wp, 10, 16, 22, 16)
}

# 10. Edit (Pencil)
Create-Icon "icon_edit" {
    param($g, $gb, $gp, $wb, $wp)
    $g.DrawPolygon($gp, @([System.Drawing.Point]::new(20,5), [System.Drawing.Point]::new(26,11), [System.Drawing.Point]::new(11,26), [System.Drawing.Point]::new(5,26), [System.Drawing.Point]::new(5,20)))
    $g.DrawLine($wp, 17, 8, 23, 14)
}

# 11. Delete (Trash)
Create-Icon "icon_delete" {
    param($g, $gb, $gp, $wb, $wp)
    $redPen = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(235, 87, 87)), 2
    $g.DrawLine($redPen, 6, 8, 26, 8)
    $g.DrawLine($redPen, 12, 5, 20, 5)
    $g.DrawRectangle($redPen, 8, 8, 16, 19)
    $g.DrawLine($redPen, 12, 12, 12, 23)
    $g.DrawLine($redPen, 16, 12, 16, 23)
    $g.DrawLine($redPen, 20, 12, 20, 23)
}

# 12. Save (Check / Disk)
Create-Icon "icon_save" {
    param($g, $gb, $gp, $wb, $wp)
    $greenPen = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(46, 204, 113)), 2.5
    $g.DrawLine($greenPen, 7, 16, 13, 22)
    $g.DrawLine($greenPen, 13, 22, 25, 10)
}

Write-Output "All icons created successfully in Assets!"
