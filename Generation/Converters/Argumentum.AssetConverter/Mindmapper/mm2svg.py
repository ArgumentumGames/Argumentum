#!/usr/bin/env python3
"""
mm2svg.py - Convert FreeMind .mm files to SVG via Graphviz

Usage:
    python mm2svg.py input.mm [output.svg] [--dot-path "C:/Program Files/Graphviz/bin/dot"]

Parses FreeMind XML, generates a Graphviz DOT file preserving colors/fonts/edges,
then renders to SVG using the 'dot' layout engine.
"""

import xml.etree.ElementTree as ET
import subprocess
import sys
import os
import re
import tempfile
from pathlib import Path


def escape_dot_label(text):
    """Escape special characters for Graphviz DOT labels."""
    if not text:
        return ""
    # Graphviz uses HTML-like labels - escape special chars
    text = text.replace("\\", "\\\\")
    text = text.replace('"', '\\"')
    text = text.replace("\n", "\\n")
    text = text.replace("\r", "")
    return text


def escape_html_label(text):
    """Escape for HTML-like labels in Graphviz."""
    if not text:
        return ""
    text = text.replace("&", "&amp;")
    text = text.replace("<", "&lt;")
    text = text.replace(">", "&gt;")
    text = text.replace('"', "&quot;")
    return text


def wrap_text(text, max_chars=30):
    """Wrap long text into multiple lines for Graphviz labels."""
    if not text or len(text) <= max_chars:
        return text
    words = text.split()
    lines = []
    current_line = ""
    for word in words:
        if current_line and len(current_line) + len(word) + 1 > max_chars:
            lines.append(current_line)
            current_line = word
        else:
            current_line = f"{current_line} {word}" if current_line else word
    if current_line:
        lines.append(current_line)
    return "\\n".join(lines)


class MindMapNode:
    """Represents a node in the FreeMind mind map."""
    def __init__(self, element, depth=0, parent=None):
        self.id = element.get("ID", f"node_{id(element)}")
        self.text = element.get("TEXT", "")
        self.style = element.get("STYLE", "fork")
        self.position = element.get("POSITION", "")
        self.link = element.get("LINK", "")
        self.bg_color = element.get("BACKGROUND_COLOR", "")
        self.text_color = element.get("COLOR", "")
        self.depth = depth
        self.parent = parent
        self.children = []

        # Parse font element
        font_el = element.find("font")
        self.font_size = int(font_el.get("size", "14")) if font_el is not None else 14
        self.font_bold = font_el is not None and font_el.get("BOLD", "false").lower() == "true"

        # Parse edge element
        edge_el = element.find("edge")
        self.edge_width = 1
        self.edge_color = ""
        if edge_el is not None:
            self.edge_width = int(edge_el.get("WIDTH", "1"))
            self.edge_color = edge_el.get("COLOR", "")

        # Parse child nodes
        for child_el in element.findall("node"):
            child = MindMapNode(child_el, depth + 1, self)
            self.children.append(child)


def collect_all_nodes(node, result=None):
    """Flatten the tree into a list of all nodes."""
    if result is None:
        result = []
    result.append(node)
    for child in node.children:
        collect_all_nodes(child, result)
    return result


def generate_dot(root_node):
    """Generate Graphviz DOT representation of the mind map."""
    lines = []
    lines.append('digraph mindmap {')
    lines.append('    // Global settings')
    lines.append('    graph [')
    lines.append('        rankdir=LR,')
    lines.append('        ranksep=1.5,')
    lines.append('        nodesep=0.3,')
    lines.append('        splines=curved,')
    lines.append('        overlap=false,')
    lines.append('        bgcolor="transparent"')
    lines.append('    ];')
    lines.append('    node [')
    lines.append('        fontname="SansSerif",')
    lines.append('        margin="0.15,0.08"')
    lines.append('    ];')
    lines.append('    edge [')
    lines.append('        arrowhead=none')
    lines.append('    ];')
    lines.append('')

    all_nodes = collect_all_nodes(root_node)

    # Separate left and right branches
    left_children = [c for c in root_node.children if c.position == "left"]
    right_children = [c for c in root_node.children if c.position != "left"]

    # Generate node definitions
    for node in all_nodes:
        attrs = []

        # Label - wrap long text
        label = wrap_text(node.text, 25)
        attrs.append(f'label="{escape_dot_label(label)}"')

        # Shape based on style
        if node.style == "bubble":
            attrs.append('shape=box')
            attrs.append('style="filled,rounded"')
        else:
            attrs.append('shape=box')
            attrs.append('style="filled,rounded"')

        # Colors
        if node.bg_color:
            attrs.append(f'fillcolor="{node.bg_color}"')
            # Determine text color based on background brightness
            attrs.append('fontcolor="white"')
        elif node.text_color:
            attrs.append(f'fontcolor="{node.text_color}"')
            attrs.append('fillcolor="#FFFFFF"')
        elif node.depth == 0:
            attrs.append('fillcolor="#E0E0E0"')
            attrs.append('fontcolor="#333333"')
        else:
            attrs.append('fillcolor="#FFFFFF"')
            attrs.append('fontcolor="#333333"')

        # Font size - scale down for Graphviz (mm font sizes are huge)
        gv_fontsize = max(8, node.font_size // 4)
        attrs.append(f'fontsize={gv_fontsize}')

        # Bold font
        if node.font_bold:
            attrs.append('fontname="SansSerif Bold"')

        # URL link
        if node.link:
            attrs.append(f'URL="{escape_dot_label(node.link)}"')
            attrs.append('target="_blank"')

        # Tooltip with full text
        attrs.append(f'tooltip="{escape_dot_label(node.text)}"')

        safe_id = f'"{node.id}"'
        lines.append(f'    {safe_id} [{", ".join(attrs)}];')

    lines.append('')
    lines.append('    // Edges')

    # Generate edges
    for node in all_nodes:
        for child in node.children:
            edge_attrs = []

            # Edge width - scale down
            gv_width = max(0.5, child.edge_width / 5.0)
            edge_attrs.append(f'penwidth={gv_width:.1f}')

            # Edge color
            if child.edge_color:
                edge_attrs.append(f'color="{child.edge_color}"')
            elif node.edge_color:
                edge_attrs.append(f'color="{node.edge_color}"')

            parent_id = f'"{node.id}"'
            child_id = f'"{child.id}"'
            lines.append(f'    {parent_id} -> {child_id} [{", ".join(edge_attrs)}];')

    lines.append('}')
    return '\n'.join(lines)


def convert_mm_to_svg(mm_path, svg_path=None, dot_path="dot"):
    """Convert a FreeMind .mm file to SVG via Graphviz.

    Args:
        mm_path: Path to the .mm file
        svg_path: Path for the output SVG (defaults to same name with .svg extension)
        dot_path: Path to the Graphviz 'dot' executable

    Returns:
        Path to the generated SVG file
    """
    mm_path = Path(mm_path)
    if svg_path is None:
        svg_path = mm_path.with_suffix('.svg')
    else:
        svg_path = Path(svg_path)

    # Parse .mm XML
    tree = ET.parse(str(mm_path))
    map_element = tree.getroot()
    root_node_element = map_element.find("node")

    if root_node_element is None:
        raise ValueError(f"No root <node> found in {mm_path}")

    # Build node tree
    root_node = MindMapNode(root_node_element)

    # Count nodes
    all_nodes = collect_all_nodes(root_node)
    print(f"  Parsed {len(all_nodes)} nodes from {mm_path.name}")

    # Generate DOT
    dot_content = generate_dot(root_node)

    # Write DOT to temp file
    dot_path_file = mm_path.with_suffix('.dot')
    with open(str(dot_path_file), 'w', encoding='utf-8') as f:
        f.write(dot_content)
    print(f"  Generated DOT file: {dot_path_file.name}")

    # Run Graphviz
    try:
        result = subprocess.run(
            [dot_path, '-Tsvg', str(dot_path_file), '-o', str(svg_path)],
            capture_output=True,
            text=True,
            timeout=120
        )
        if result.returncode != 0:
            print(f"  ERROR: Graphviz failed: {result.stderr}", file=sys.stderr)
            return None
    except FileNotFoundError:
        print(f"  ERROR: Graphviz 'dot' not found at '{dot_path}'", file=sys.stderr)
        print("  Install Graphviz or provide --dot-path argument", file=sys.stderr)
        return None
    except subprocess.TimeoutExpired:
        print(f"  ERROR: Graphviz timed out (120s)", file=sys.stderr)
        return None

    # Check output
    if svg_path.exists() and svg_path.stat().st_size > 0:
        size_kb = svg_path.stat().st_size / 1024
        print(f"  Generated SVG: {svg_path.name} ({size_kb:.0f} KB)")
        return str(svg_path)
    else:
        print(f"  ERROR: SVG file not generated or empty", file=sys.stderr)
        return None


def main():
    if len(sys.argv) < 2:
        print("Usage: python mm2svg.py input.mm [output.svg] [--dot-path PATH]")
        sys.exit(1)

    mm_path = sys.argv[1]
    svg_path = None
    dot_exe = "dot"

    # Parse arguments
    args = sys.argv[2:]
    i = 0
    while i < len(args):
        if args[i] == "--dot-path" and i + 1 < len(args):
            dot_exe = args[i + 1]
            i += 2
        elif not svg_path:
            svg_path = args[i]
            i += 1
        else:
            i += 1

    print(f"Converting {mm_path} to SVG...")
    result = convert_mm_to_svg(mm_path, svg_path, dot_exe)

    if result:
        print(f"Success: {result}")
    else:
        print("Conversion failed.", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()
