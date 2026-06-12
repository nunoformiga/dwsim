# MDH Native DWSIM Model Plan

This branch is for moving the MDH case away from embedded Python-script
surrogates and into reusable DWSIM-native behavior. Use existing DWSIM unit
operation names in user-visible model structure and documentation.

## Native Unit Operation Targets

| MDH tag | Current role | DWSIM unit operation name |
|---|---|---|
| M-101 | Blend MgSO4 and NaOH feeds | Mixer |
| R-101 | Mg(OH)2 precipitation | Conversion Reactor, then Gibbs Reactor (Reaktoro) when electrolyte equilibrium is validated |
| E-101 | Trim slurry temperature | Cooler |
| R-102 | Aging residence volume | Tank |
| S-101 | Thickener / clarifier split | Solids Separator |
| R-103 | Hydrothermal heat treatment | Heater, with Tank or Vessel residence behavior if needed |
| W-201 | Reslurry wash addition | Mixer, followed by Filter or Solids Separator when wash displacement is modeled |
| C-101 | Washed cake / filtrate split | Filter |
| TK-201 | Combine mother liquors | Mixer |
| D-101 | Dry cake and remove water vapor | Heater plus Separator |
| M-102 | De-agglomeration / classification loss | Solids Separator |
| CT-101 | Add stearate to product solids | Mixer |
| WW-101 | Purge wastewater stream | Splitter |

## Thermodynamics Target

Start from DWSIM's existing electrolyte/Reaktoro infrastructure:

- Reaktoro (Aqueous Electrolytes) Property Package
- Gibbs Reactor (Reaktoro)
- IdealElectrolytePropertyPackage / ElectrolyteBasePropertyPackage only where the species and parameters are adequate

The predictive model should use ionic species and solids instead of neutral
apparent components where the DWSIM/Reaktoro data support it:

- H2O
- Na+
- Mg+2
- SO4-2
- HSO4-
- OH-
- H+
- MgOH+
- MgSO4(aq)
- NaSO4-
- Mg(OH)2(s)

## Validation Rule

The current MDH project can remain the regression harness, but validation must
not only compare one base-case stream table. A reusable model needs multiple
cases covering feed concentration, NaOH ratio, temperature, residence time,
wash ratio, filter settings, dryer duty, product moisture, and measured
residual Mg/Na/SO4.

## Implementation Rule

Do not introduce user-visible custom unit operation names for standard
equipment. Extend existing DWSIM units, property packages, calculation modes,
or model options where possible. Only add new DWSIM object types when no
existing unit operation name fits the equipment.
