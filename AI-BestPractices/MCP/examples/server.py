from mcp.server.fastmcp import FastMCP

# Create MCP server instance
mcp = FastMCP("Simple Calculator Server")

# Define a tool
@mcp.tool()
def add_numbers(a: float, b: float) -> float:
    """
    Adds two numbers and returns the result.
    """
    return a + b

@mcp.tool()
def minus_numbers(a: float, b: float) -> float:
    """
    Subtracts the second number from the first and returns the result.
    """
    return a - b

# Run the server
if __name__ == "__main__":
    mcp.run()
