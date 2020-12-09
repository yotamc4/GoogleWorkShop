import * as React from "react";
import * as FormsStyles from "../FormStyles/FormsStyles";
import { Stack, Dropdown, Label } from "office-ui-fabric-react";
import {
  DefaultButton,
  DetailsList,
  DetailsListLayoutMode,
  FontIcon,
  IColumn,
  IIconProps,
  mergeStyles,
  mergeStyleSets,
  Persona,
  PersonaSize,
  PrimaryButton,
  SelectionMode,
} from "@fluentui/react";
import { Buyer } from "../Modal/Buyers";

const iconClass = mergeStyles({
  fontSize: 30,
  height: 30,
  width: 30,
});

const classNames = mergeStyleSets({
  greenYellow: [{ color: "greenyellow" }, iconClass],
  red: [{ color: "indianRed" }, iconClass],
});

const payIcon: IIconProps = {
  iconName: "Money",
  styles: { root: { fontSize: "2rem" } },
};

export const PaymentsTable: React.FunctionComponent<{
  payers: Buyer[];
}> = (props) => {
  const columns: IColumn[] = [
    {
      key: "column2",
      name: "Name",
      fieldName: "name",
      minWidth: 210,
      maxWidth: 350,
      isRowHeader: true,
      isResizable: true,
      isSorted: true,
      isSortedDescending: false,
      sortAscendingAriaLabel: "Sorted A to Z",
      sortDescendingAriaLabel: "Sorted Z to A",
      data: "string",
      isPadded: true,
      onRender: (payer: Buyer) => (
        <Persona
          imageUrl={payer.imageUrl}
          text={payer.name}
          size={PersonaSize.size56}
        />
      ),
    },
    {
      key: "column3",
      name: "Payed",
      fieldName: "Payed",
      minWidth: 70,
      maxWidth: 90,
      isResizable: true,
      data: "Icon",
      onRender: (payer: Buyer) => {
        if (payer.paid) {
          return (
            <FontIcon iconName="Accept" className={classNames.greenYellow} />
          );
        } else {
          return (
            <FontIcon
              iconName="CalculatorMultiply"
              className={classNames.red}
            />
          );
        }
      },
      isPadded: true,
    },
  ];

  return (
    <Stack horizontalAlign={"center"}>
      <PrimaryButton
        text={"Pay with Payapl"}
        iconProps={payIcon}
        href={"https://www.paypal.com"}
        styles={{
          root: { width: "24rem", height: "3.5rem", fontSize: "1.5 rem" },
        }}
      />
      <Label styles={{ root: { fontSize: 30, color: "#605e5c" } }}>
        Current payments status:
      </Label>
      <DetailsList
        items={props.payers}
        columns={columns}
        selectionMode={SelectionMode.none}
        setKey="none"
        layoutMode={DetailsListLayoutMode.justified}
        isHeaderVisible={true}
      />
    </Stack>
  );
};
