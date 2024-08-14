using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeuronLogic
{
    public static int And(int A, int B)
    {
        return (A > 0 && B > 0) ? 1 : -1;
    }
    public static int Not(int A, int B)
    {
        return A > 0 ? -1 : 1;
    }

    // Excitatory Neuron as an OR gate, returning 1 or -1
    public static int ExcitatoryOr(int A, int B)
    {
        return (A > 0 || B > 0) ? 1 : -1;
    }

    // Inhibitory Neuron as a NOT gate, returning 1 or -1
    public static int InhibitoryNot(int A)
    {
        return A > 0 ? -1 : 1;
    }

    // Mutual Excitation
    public static int MutualExcitation(int inputA, int inputB)
    {
        return (inputA > 0 || inputB > 0) ? 1 : -1;
    }

    // Mutual Inhibition
    public static int MutualInhibition(int A, int B)
    {
        return (A > 0 && B > 0) ? -1 : 1;
    }

    // Feedforward Inhibition
    public static int FeedforwardInhibition(int A, int B)
    {
        return (A > 0 && B <= 0) ? 1 : -1;
    }

    // Feedback Inhibition
    public static int FeedbackInhibition(int input)
    {
        return input > 0 ? -1 : 1;
    }

    // Excitatory-Inhibitory Feedback
    public static int ExcitatoryInhibitoryFeedback(int excitatoryInput, int inhibitoryInput)
    {
        return (excitatoryInput > 0 && inhibitoryInput <= 0) ? 1 : -1;
    }

    // Inhibitory Feedback Loop
    public static int InhibitoryFeedbackLoop(int input)
    {
        return input > 0 ? -1 : 1;
    }

    // Excitatory-Inhibitory Cascade
    public static int ExcitatoryInhibitoryCascade(int excitatoryInput, int inhibitoryInput)
    {
        int intermediate = ExcitatoryOr(excitatoryInput, inhibitoryInput);
        return InhibitoryNot(intermediate);
    }

    // Complex Network with Multiple Inputs
    public static int ComplexNetwork(int input1, int input2, int input3)
    {
        int intermediate1 = ExcitatoryOr(input1, input2);
        int intermediate2 = InhibitoryNot(input3);
        return ExcitatoryOr(intermediate1, intermediate2);
    }

    // Sequential Activation
    public static int SequentialActivation(int inputA, int inputB, int inputC)
    {
        int firstStage = ExcitatoryOr(inputA, inputB);
        return ExcitatoryOr(firstStage, inputC);
    }




}
