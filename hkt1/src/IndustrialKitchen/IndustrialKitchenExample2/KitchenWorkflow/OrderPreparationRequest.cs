using System;
using System.Linq;

namespace IndustrialKitchenExample2.KitchenWorkflow
{
    public class OrderPreparationRequest
    {
        public DishPreparationOrder Order { get; internal set; }
        public PreparationRequest PreparationRequestStatus { get; internal set; }

        public RecipeStep StartExecution()
        {
            this.PreparationRequestStatus = PreparationRequest.WorkInProgress;
            this.Order.PreparationStartTime = DateTime.Now;
            var firstStep = this.Order.Recipe.GetFirstStep();
            return firstStep;
        }

        public RecipeStep CompleteStep(RecipeStep step)
        {
            var steps = this.Order.Recipe.Steps.ToList();
            step.SetCompleteTime();

            var nextStepId = steps.IndexOf(step) + 1;
            var nextStep = step;

            if (nextStepId <= steps.Count -1)
            {
                nextStep = steps[nextStepId];
                nextStep.SetStartTime();
            }
            else
            {
                this.Order.PreparationEndTime = DateTime.Now;
                this.PreparationRequestStatus = PreparationRequest.Completed;
            }

            
            return nextStep;
        }

    }
}
