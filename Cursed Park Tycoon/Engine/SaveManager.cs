using Microsoft.Xna.Framework;
using Sandbox.Oyun.Inventory_System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sandbox.Engine
{
    public class SaveManager
    {
        //== WORLD-ENTITY SAVE SYSTEM ==//
        public static void Save<T>(string fullFileName, List<T> savedlist)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty-printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };
            options.Converters.Add(new Vector2JsonConverter());

            string serializedContent = JsonSerializer.Serialize(savedlist, options);

            File.WriteAllText(fullFileName, serializedContent);
        }
        public static List<T> Load<T>(string fullFileName)
        {
            if (!File.Exists(fullFileName))
            {
                File.Create(fullFileName);
                return new List<T>();
            }

            var options = new JsonSerializerOptions();
            options.Converters.Add(new Vector2JsonConverter());

            string serializedContent = File.ReadAllText(fullFileName);
            List<T> entities = JsonSerializer.Deserialize<List<T>>(serializedContent, options);

            entities ??= new List<T>();

            return entities;
        }

        //== INVENTORY SAVE SYSTEM ==//
        public static void SaveInventory<ItemSlot>(string fullFileName, ref List<ItemSlot> savedlist)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty-printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            string serializedContent = JsonSerializer.Serialize(savedlist, options);

            File.WriteAllText(fullFileName, serializedContent);
        }
        public static List<ItemSlot> LoadInventory<T>(string fullFileName)
        {
            if (!File.Exists(fullFileName))
            {
                File.Create(fullFileName);
                return new List<ItemSlot>();
            }

            var options = new JsonSerializerOptions();

            string serializedContent = File.ReadAllText(fullFileName);
            List<ItemSlot> itemSlots = JsonSerializer.Deserialize<List<ItemSlot>>(serializedContent, options);

            itemSlots ??= new List<ItemSlot>();

            return itemSlots;
        }
    }

    // Çünkü Vector2 tipini bilmiyordu JSON. Biz kendimiz dönüştürücü yaptık.
    class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            float x = 0;
            float y = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Vector2(x, y);
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();

                    reader.Read();

                    switch (propertyName)
                    {
                        case "X":
                            x = reader.GetSingle();
                            break;
                        case "Y":
                            y = reader.GetSingle();
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteEndObject();
        }
    }
}
