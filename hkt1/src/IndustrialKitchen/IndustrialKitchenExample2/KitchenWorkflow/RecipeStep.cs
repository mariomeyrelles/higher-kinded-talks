using System;

namespace IndustrialKitchenExample2.KitchenWorkflow
{
    public class RecipeStep
    {

        public RecipeStep(string description, bool isLastStep = false)
        {
            this.Description = description;
            this.IsLastStep = isLastStep;
        }

        public bool IsLastStep { get; }
        public string Description { get; }
        public DateTime StartedAt { get; protected set; }
        public DateTime CompletedAt { get; protected set; }

        public void SetStartTime()
        {
            this.StartedAt = DateTime.Now;
        }

        public void SetCompleteTime()
        {
            this.CompletedAt = DateTime.Now;
        }
    }
}
