using Autostyle.Items.Robots;
using Autostyle.Projects;
using SignsOfLife.Crafting;
using SignsOfLife.Entities.Items;
using SignsOfLife.Prefabs.StaticPrefabs;
using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using System.Linq;

namespace Autostyle
{
    public sealed class Recipes
    {

        public static Recipe RecipeTechBlowbot { get; private set; }

        public static Blueprint BlueprintIndustrialDrill { get; private set; }
        public static Blueprint BlueprintCombustionGenerator { get; private set; }

        public RequiredRecipeMaterial[] DebugNoMats = new RequiredRecipeMaterial[] { new RequiredRecipeMaterial(InventoryItemType.TECH_BLOWER, 1) };

        public Recipes()
        {
            RecipeTechBlowbot = new Recipe();
            RecipeTechBlowbot.Name = "Vacubot";
            RecipeTechBlowbot.AutoGrant = true;
            RecipeTechBlowbot.CraftingCategory = "Gadgets";
            RecipeTechBlowbot.Category = "Gadgets";
            RecipeTechBlowbot.ResultItem = new ResultRecipeItem((InventoryItemType)TechBlowbot.ID,1);
            //RecipeTechBlowbot.RequiredMaterials = new RequiredRecipeMaterial[]{
            //    new RequiredRecipeMaterial(InventoryItemType.TECH_BLOWER,1),
            //    new RequiredRecipeMaterial(InventoryItemType.MICROCHIP,5),
            //    new RequiredRecipeMaterial(InventoryItemType.WIRE_COPPER,25),
            //    new RequiredRecipeMaterial(InventoryItemType.MACHINE_PARTS_STEEL,2),
            //    new RequiredRecipeMaterial(InventoryItemType.RIVET_STEEL,10)
            //}.ToList();
            RecipeTechBlowbot.RequiredMaterials = DebugNoMats.ToList();
            RecipeTechBlowbot.AdditionalRequirement = "Material Printer";

            BlueprintIndustrialDrill = new Blueprint();
            BlueprintIndustrialDrill.Name = IndustrialDrill.TypeName;
            BlueprintIndustrialDrill.Description = "Enjoy your life while others work.";
            BlueprintIndustrialDrill.Hidden = false;
            BlueprintIndustrialDrill.ResultStaticPrefab = 
                new ResultBlueprintStaticPrefab((StaticPrefabType)IndustrialDrill.TypeID);
            BlueprintIndustrialDrill.RequiredMaterials = new RequiredRecipeMaterial[]{

            }.ToList();

            BlueprintCombustionGenerator = new Blueprint();
            BlueprintCombustionGenerator.Name = CombustionGenerator.TypeName;
            BlueprintCombustionGenerator.Description = "As positive as possible.";
            BlueprintCombustionGenerator.Hidden = false;
            BlueprintCombustionGenerator.ResultStaticPrefab = new ResultBlueprintStaticPrefab((StaticPrefabType)CombustionGenerator.TypeID);
            BlueprintCombustionGenerator.RequiredMaterials = new RequiredRecipeMaterial[]{

            }.ToList();


        }

        public void InjectAll()
        {
            RecipeRegistry.Instance.ForceInject(RecipeTechBlowbot);
            RecipeRegistry.Instance.ForceInject(BlueprintIndustrialDrill);
            RecipeRegistry.Instance.ForceInject(BlueprintCombustionGenerator);
        }

    }
}
