require 'HybridCalc.Contracts'

IErrorLog = HybridCalc::Contracts::IErrorLog
IOperation = HybridCalc::Contracts::IOperation

class Multiply < PartDefinition
  export IOperation
  
  def Symbol
	"*"
  end
  
  def Apply(a, b)
	a * b
  end
end

class Divide < PartDefinition
  export IOperation
  import IErrorLog, :into => :@error_log
  
  def Symbol
	"/"
  end
  
  def Apply(a, b)
    if b == 0
      @error_log.add_message("Cannot divide by zero.")
      System::Double.NaN
    else
	  a / b
	end
  end
end
