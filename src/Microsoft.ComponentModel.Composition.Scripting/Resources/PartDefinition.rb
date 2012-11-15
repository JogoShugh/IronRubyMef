Composition = Microsoft::ComponentModel::Composition::Scripting
RubyPartDefinition = Composition::RubyPartDefinition
RubyPart = Composition::RubyPart
RubyExportDefinition = Composition::RubyExportDefinition
RubyImportDefinition = Composition::RubyImportDefinition
Export = System::ComponentModel::Composition::Primitives::Export[]

class Export
  def method_missing(mname, *args, &block)
    get_exported_object.send(mname, *args, &block)
  end
end

class PartDefinition
  
  def self.export(cname, attrs={})
    if cname.is_a?(Module)
	    include cname
	end
	
    export_attr(cname, attrs) do
      self
    end
  end
  
  def self.export_attr(cname, attrs={}, &block)
    export_accessor = proc { |p| p.instance_eval(&block) }
    add_export(create_export_def(cname, attrs, export_accessor))
    nil
  end  
  
  def self.export_method(cname, attrs={}, &block)
  	mname = "__export_#{cname}"
	define_method(mname, &block)
    export_attr(cname, attrs) do
	  p = self
      proc { |*args| p.send(mname, *args, &block) }
    end
  end
  
  def self.import(cname, attrs={}, &block)
	mname = "__import_#{cname}"
	if block_given?
		define_method(mname, &block)
	else
	    define_method(mname, create_import_block(attrs))
	end
    import_mutator = proc { |p, v| p.send(mname, v) }
    add_import(create_import_def(cname, attrs, import_mutator))
    nil
  end
  
private

  def self.add_export(export_def)
    part_definition.add_export_definition(export_def)
  end
  
  def self.add_import(import_def)
    part_definition.add_import_definition(import_def)
  end
  
  def self.part_definition
    part_def = nil
    if not MefPartsCollection.contains_key(self)
      create_part_instance = proc { self.new }
      create_part = proc { RubyPart.new(part_def, create_part_instance) }
      part_def = RubyPartDefinition.new(self.to_s, create_part)
      MefPartsCollection.add(self, part_def)     
    else
      part_def = MefPartsCollection.item(self)
    end
    part_def
  end

  def self.create_export_def(cname, attrs, accessor)
    RubyExportDefinition.new(contract_name(cname), nil, accessor)
  end
  
  def self.create_import_def(cname, attrs, mutator)
    RubyImportDefinition.new(contract_name(cname), mutator)
  end
  
  def self.contract_name(contract)
    if contract.is_a?(Module)
      contract.to_s.gsub('::', '.')
	else
	  contract.to_s
	end
  end
  
  def self.create_import_block(attrs)
    attr_sym = attrs[:into]
    raise 'Either a block or an :into => attribute must be specified' unless attr_sym
    proc { |i| self.instance_variable_set(attr_sym, i) }
  end
end
