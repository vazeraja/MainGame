using System;
using UnityEngine;


public class SaveManager : MonoBehaviour {
    private readonly IChain chainCalc1 = new AddNumbers();
    private readonly IChain chainCalc2 = new SubtractNumbers();
    private readonly IChain chainCalc3 = new MultiplyNumbers();

    private Numbers request = new Numbers(4, 2, "mult");
    
    private void Start() {
        chainCalc1.SetNextChain(chainCalc2);
        chainCalc2.SetNextChain(chainCalc3);
        
        // chainCalc1.Calculate(request);
    }
}

public interface IChain {
    void SetNextChain(IChain nextChain);
    void Calculate(Numbers request);
}

public class Numbers {
    private readonly int num1;
    private readonly int num2;

    private readonly string calculationWanted;

    public Numbers(int newNumber1, int newNumber2, string calcWanted) {
        num1 = newNumber1;
        num2 = newNumber2;
        calculationWanted = calcWanted;
    }

    public int GetNum1() => num1;
    public int GetNum2() => num2;
    public string GetCalcWanted() => calculationWanted;
}

public class AddNumbers : IChain {
    private IChain nextInChain;

    public void SetNextChain(IChain nextChain) {
        this.nextInChain = nextChain;
    }

    public void Calculate(Numbers request) {
        if (request.GetCalcWanted() == "add") {
            Debug.Log(request.GetNum1() + " + " + request.GetNum2() +
                      " = " + (request.GetNum1() + request.GetNum2()));
        }
        else {
            Debug.Log("Request was not an addition request");
            nextInChain.Calculate(request);
        }
    }
}

public class SubtractNumbers : IChain {
    private IChain nextInChain;

    public void SetNextChain(IChain nextChain) {
        this.nextInChain = nextChain;
    }

    public void Calculate(Numbers request) {
        if (request.GetCalcWanted() == "sub") {
            Debug.Log(request.GetNum1() + " - " + request.GetNum2() +
                      " = " + (request.GetNum1() - request.GetNum2()));
        }
        else {
            Debug.Log("Request was not an subtraction request");
            nextInChain.Calculate(request);
        }
    }
}

public class MultiplyNumbers : IChain {
    private IChain nextInChain;

    public void SetNextChain(IChain nextChain) {
        this.nextInChain = nextChain;
    }

    public void Calculate(Numbers request) {
        if (request.GetCalcWanted() == "mult") {
            Debug.Log(request.GetNum1() + " * " + request.GetNum2() +
                      " = " + (request.GetNum1() * request.GetNum2()));
        }
        else {
            Debug.Log("Request was not an multiplication request");
        }
    }
}