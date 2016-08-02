echo off
echo INSTALANDO REPORTES

echo "reporte1.rdl" 
rs -i UploadReport.rss -s http://servidor/SSRS -v path="/CarpetaReportes"  -v reportName="reporte1"  -v reportSourceFile="reporte1.rdl"

echo "reporte2.rdl" 
rs -i UploadReport.rss -s http://servidor/SSRS -v path="/CarpetaReportes"  -v reportName="reporte2"  -v reportSourceFile="reporte2.rdl"

pause
