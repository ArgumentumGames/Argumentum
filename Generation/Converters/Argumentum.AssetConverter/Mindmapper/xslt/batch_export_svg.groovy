// Batch SVG export script for Freeplane
// Usage: freeplane.bat -R"path/to/batch_export_svg.groovy" file.mm
import javax.swing.SwingUtilities
import java.util.concurrent.CountDownLatch

def mmFile = node.map.file
if (mmFile == null) {
    System.err.println("ERROR: No map file loaded")
    System.exit(1)
}

def svgFile = new File(mmFile.path.replaceFirst('\\.mm$', '.svg'))
System.err.println("Exporting: ${mmFile.name} -> ${svgFile.name}")

try {
    // Wait a moment for full rendering
    Thread.sleep(3000)

    c.export(node.map, svgFile, 'Scalable Vector Graphic (SVG) (.svg)', true)

    if (svgFile.exists() && svgFile.length() > 0) {
        System.err.println("SUCCESS: ${svgFile.name} (${svgFile.length() / 1024} KB)")
    } else {
        System.err.println("FAILED: SVG file not created or empty")
    }
} catch (Exception e) {
    System.err.println("ERROR: ${e.message}")
}

// Signal completion by creating a marker file
new File(mmFile.parent, '.export_done').text = svgFile.name
