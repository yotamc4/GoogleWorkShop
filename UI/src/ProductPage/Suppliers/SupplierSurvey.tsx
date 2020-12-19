import * as React from "react";
import {
  ChoiceGroup,
  IChoiceGroupOption,
} from "office-ui-fabric-react/lib/ChoiceGroup";
import { Stack, Text, DefaultButton } from "@fluentui/react";
import {
  choiceGroupStyles,
  defaultButtonVoteStyles,
  textStyles,
  verticalGapStackTokens,
} from "./SupplierSurveyStyles";

export interface ISuppliersSurveyProps {
  supliersNames: IChoiceGroupOption[] | undefined;
}

export const SuppliersSurvey: React.FunctionComponent<ISuppliersSurveyProps> = ({
  supliersNames,
}) => {
  const [selectedKey, setSelectedKey] = React.useState<string>();

  const onChange = React.useCallback(
    (
      evt?: React.FormEvent<HTMLElement | HTMLInputElement>,
      option: IChoiceGroupOption | undefined = undefined
    ) => {
      setSelectedKey(option?.key as string);
    },
    []
  );

  return (
    <Stack tokens={verticalGapStackTokens}>
      <Text
        block={true}
        className="Bold"
        styles={textStyles}
        variant="xLargePlus"
      >
        Time to choose you're favorite supplier
      </Text>
      <ChoiceGroup
        styles={choiceGroupStyles}
        selectedKey={selectedKey}
        options={supliersNames}
        onChange={onChange}
      />
      <DefaultButton
        text={"Vote"}
        primary
        styles={defaultButtonVoteStyles}
      ></DefaultButton>
    </Stack>
  );
};
