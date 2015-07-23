using Autostyle.Items;
using Autostyle.Items.Robots;
using Autostyle.Projects;
using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using SOLPolymorph.SignsOfLife.Polymorph.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostyle
{
    public class Autostyle : SoLMod
    {

        public override void InitItems(ItemRegistry registry)
        {
            registry.Register<TrayBasket>(TrayBasket.ID);
            registry.Register<TechBlowbot>(TechBlowbot.ID);
            registry.Register<ChargableBattery>(ChargableBattery.ID);
        }

        public override void InitRecipes(RecipeRegistry registry)
        {
            Recipes recipes = new Recipes();
            recipes.InjectAll();
        }

        public override void InitProjects(ProjectRegistry registry)
        {
            registry.Register<IndustrialDrill>(IndustrialDrill.TypeID);
            registry.Register<CombustionGenerator>(CombustionGenerator.TypeID);
        }
    }
}
