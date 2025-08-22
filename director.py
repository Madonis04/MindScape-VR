import os
import json
import google.generativeai as genai
from dotenv import load_dotenv

# --- SETUP ---
# Load environment variables and configure the API
load_dotenv()
try:
    api_key = os.getenv("GOOGLE_API_KEY")
    if not api_key:
        raise ValueError("❌ GOOGLE_API_KEY not found. Please set it in the .env file.")
    genai.configure(api_key=api_key)
    print("✅ Gemini client configured successfully.")
except Exception as e:
    print(f"❌ Error configuring Gemini client: {e}")
    exit()

# --- PROMPT ENGINEERING ---
# This is our detailed instruction set for the AI
# --- PROMPT ENGINEERING ---
# ...existing code...
PROMPT = """
You are a creative director for a Unity 3D game engine. Your task is to read a descriptive text and convert it into a structured JSON object.

Identify the key objects in the text. For each atom in a molecule, provide a 'name', a suggested 'asset_id', a 'position' [x, y, z] based on realistic molecular geometry (e.g., tetrahedral angles for carbon atoms, typical bond lengths ~1.5 units), a 'scale' [x, y, z], and a 'color' as an RGBA array [r, g, b, a] with values from 0.0 to 1.0.

CRITICAL: The final output must be a single JSON object with one key: "scene_objects", which contains an array of the identified objects.
The structure must be: { "scene_objects": [ ... ] }

Respond ONLY with the raw JSON object. Do not include any other text.
"""
# ...existing code...

def generate_scene_script(text_input):
    """
    Sends the text and prompt to the Gemini API to generate a scene script.
    """
    try:
        print("\nSending text to the AI Director...")
        model = genai.GenerativeModel('gemini-2.5-flash')
        
        # Combine the main prompt with the user's specific text
        full_prompt = f"{PROMPT}\n\nTEXT TO ANALYZE: \"{text_input}\""
        
        response = model.generate_content(full_prompt)
        
        # Clean up the response to make sure it's valid JSON
        # The AI sometimes wraps the JSON in markdown backticks
        cleaned_response = response.text.strip().replace("```json", "").replace("```", "")
        
        print("✅ Received scene script from AI:")
        # Parse and re-print for nice formatting
        parsed_json = json.loads(cleaned_response)
        print(json.dumps(parsed_json, indent=2))

        # We can also save this to a file for Unity to read later
        with open(r"D:\Unity Projects\MCP Trial\Assets\Resources\scene_script.json", "w") as f:
            json.dump(parsed_json, f, indent=2)
        print("\n✅ Scene script saved to 'scene_script.json'")

        return cleaned_response

    except Exception as e:
        print(f"❌ An error occurred: {e}")
        return None

# --- EXECUTION ---
if __name__ == "__main__":
    # This is the sample text we want to turn into a 3D world
    sample_text = "Create a 3D Model of Solar System with a central Sun and orbiting planets."
    
    generate_scene_script(sample_text)